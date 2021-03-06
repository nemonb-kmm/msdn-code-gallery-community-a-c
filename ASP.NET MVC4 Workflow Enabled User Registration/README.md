# ASP.NET MVC4 Workflow Enabled User Registration
## Requires
- Visual Studio 2012
## License
- Apache License, Version 2.0
## Technologies
- Windows Workflow
- WF4
- ASP.NET MVC 4
## Topics
- Workflow
## Updated
- 05/07/2012
## Description

<div>One thing that many web sites do is to verify email addresses by sending you an email to complete registration.&nbsp; I decided to build a Registration system for ASP.NET MVC using Windows Workflow Foundation.</div>
<div>When you create a new ASP.NET MVC web site, the site comes with a simple account controller that integrates with ASP.NET Membership.&nbsp; It provides basic one step registration and log-in support.&nbsp; I wanted to take this much farther and provide
 a simple self-contained registration verification system.</div>
<h2>Scenarios</h2>
<div>When I plan work like this, my first step is to prepare the list of scenarios I'm working on so I don't get distracted and don't miss anything important</div>
<table style="padding:2px; width:100%">
<thead>
<tr>
<th style="width:262px; vertical-align:top">Given </th>
<th style="width:122px; vertical-align:top">When </th>
<th style="width:476px; vertical-align:top">Then </th>
</tr>
</thead>
<tbody>
<tr>
<td style="width:262px; vertical-align:top">
<div>A user registers for the site with a valid email address</div>
</td>
<td style="width:122px; vertical-align:top">
<div dir="ltr" style="margin-right:0px">The user clicks on Register</div>
</td>
<td style="width:476px; vertical-align:top">
<ul>
<li>The user is added to the Membership database with the isApproved flag set to false
</li><li>A Workflow is started to manage the membership verification </li><li>The workflow sends an email to the members email address </li></ul>
</td>
</tr>
<tr>
<td style="width:262px; vertical-align:top">
<div dir="ltr" style="margin-right:0px">A user receives the email verification message</div>
</td>
<td style="width:122px; vertical-align:top">The user clicks the link in the verification email</td>
<td style="width:476px; vertical-align:top">
<ul>
<li>A browser launches and opens the Verification page providing a verificationCode in the URL query string
</li><li>The Workflow is loaded and resumed with the confirmation command </li><li>The membership is approved </li></ul>
</td>
</tr>
<tr>
<td style="width:262px; vertical-align:top">A user attempts to log-in after registration but before the verification email is confirmed</td>
<td style="width:122px; vertical-align:top">The user clicks on log in</td>
<td style="width:476px; vertical-align:top">
<ul>
<li>The Membership is found in the membership database </li><li>Because the isApproved flag is false, an error is generated including a link to the page to re-send the verification email
</li></ul>
</td>
</tr>
<tr>
<td style="width:262px; vertical-align:top">A user cannot find the verification email and wants it sent again. The user navigates to the site, enters username and password and clicks on log in then clicks on the link in the error message to navigate to the
 re-send confirmation page</td>
<td style="width:122px; vertical-align:top">The user clicks re-send to send the message again</td>
<td style="width:476px; vertical-align:top">
<ul>
<li>The Workflow instanceId for the email is located using a Promoted Property. If not found, an error is displayed for an invalid email address
</li><li>The Workflow is loaded and the send mail command is resumed </li></ul>
</td>
</tr>
<tr>
<td style="width:262px; vertical-align:top">After registration, the user fails to click on the link in the confirmation mail</td>
<td style="width:122px; vertical-align:top">The timeout interval expires</td>
<td style="width:476px; vertical-align:top">
<ul>
<li>The Workflow with an expired timer is detected and the workflow is loaded </li><li>The timeout action increments a counter and a second, third or fourth message can be sent to the user
</li><li>If the timeout has exceeded the maximum number of timeouts, the user account is deleted
</li></ul>
</td>
</tr>
<tr>
<td style="width:262px; vertical-align:top">After registration, the user decides to cancel registration</td>
<td style="width:122px; vertical-align:top">The user clicks the cancel link in the email</td>
<td style="width:476px; vertical-align:top">
<ul>
<li>The Workflow is loaded </li><li>The workflow is resumed with the cancel command </li><li>The user account is deleted </li></ul>
</td>
</tr>
</tbody>
</table>
<h3>Implementation</h3>
<div>For my platform, I chose Visual Studio 11, .NET 4.5 and ASP.NET MVC 4.&nbsp; However the same concepts will work fine with .NET 4.0 and MVC 3 given a few minor modifications.</div>
<h3>Step 1 - Creating users with isApproved False</h3>
<div>For this step I simply searched account controller for isApproved. I found that by default when users are created, isApproved is set to true.&nbsp; In MVC 4 there are two methods that create users they are Register and JsonRegister. The modification is
 shown below.</div>
<div>
<div class="scriptcode">
<div class="pluginEditHolder" pluginCommand="mceScriptCode">
<div class="title"><span>C#</span></div>
<div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div>
<span class="hidden">csharp</span>

<div class="preview">
<pre class="csharp"><span class="cs__com">///&nbsp;&lt;summary&gt;</span>&nbsp;
<span class="cs__com">///&nbsp;Provides&nbsp;registration&nbsp;support&nbsp;for&nbsp;the&nbsp;registration&nbsp;pop-up&nbsp;dialog</span>&nbsp;
<span class="cs__com">///&nbsp;&lt;/summary&gt;</span>&nbsp;
<span class="cs__com">///&nbsp;&lt;param&nbsp;name=&quot;model&quot;&gt;The&nbsp;model&lt;/param&gt;</span>&nbsp;
<span class="cs__com">///&nbsp;&lt;returns&gt;An&nbsp;action&nbsp;result&lt;/returns&gt;</span>&nbsp;
<span class="cs__com">///&nbsp;&lt;remarks&gt;</span>&nbsp;
<span class="cs__com">///&nbsp;The&nbsp;default&nbsp;implementation&nbsp;of&nbsp;this&nbsp;method&nbsp;creates&nbsp;and&nbsp;automatically&nbsp;approves&nbsp;users.&nbsp;&nbsp;In&nbsp;this&nbsp;case&nbsp;we&nbsp;don't&nbsp;want&nbsp;to&nbsp;approve&nbsp;a&nbsp;user&nbsp;until&nbsp;their&nbsp;email&nbsp;is&nbsp;verified.&nbsp;&nbsp;</span>&nbsp;
<span class="cs__com">///&nbsp;The&nbsp;default&nbsp;implementation&nbsp;also&nbsp;implicitly&nbsp;logs&nbsp;in&nbsp;the&nbsp;created&nbsp;user.&nbsp;&nbsp;In&nbsp;this&nbsp;case&nbsp;we&nbsp;do&nbsp;not&nbsp;want&nbsp;to&nbsp;log&nbsp;in&nbsp;the&nbsp;newly&nbsp;created&nbsp;user.</span>&nbsp;
<span class="cs__com">///&nbsp;&lt;/remarks&gt;</span>&nbsp;
[AllowAnonymous]&nbsp;
[HttpPost]&nbsp;
<span class="cs__keyword">public</span>&nbsp;ActionResult&nbsp;JsonRegister(RegisterModel&nbsp;model)&nbsp;
{&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__keyword">if</span>&nbsp;(<span class="cs__keyword">this</span>.ModelState.IsValid)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__com">//&nbsp;Attempt&nbsp;to&nbsp;register&nbsp;the&nbsp;user</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;MembershipCreateStatus&nbsp;createStatus;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__com">//&nbsp;TODO:&nbsp;Notice&nbsp;how&nbsp;we&nbsp;set&nbsp;isApproved&nbsp;=&nbsp;false&nbsp;until&nbsp;email&nbsp;verification&nbsp;is&nbsp;complete</span>&nbsp;

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Membership.CreateUser(&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;model.UserName,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;model.Password,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;model.Email,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;passwordQuestion:&nbsp;<span class="cs__keyword">null</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;passwordAnswer:&nbsp;<span class="cs__keyword">null</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;isApproved:&nbsp;<span class="cs__keyword">false</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;providerUserKey:&nbsp;<span class="cs__keyword">null</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;status:&nbsp;<span class="cs__keyword">out</span>&nbsp;createStatus);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__keyword">if</span>&nbsp;(createStatus&nbsp;==&nbsp;MembershipCreateStatus.Success)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__com">//&nbsp;TODO:&nbsp;Notice&nbsp;how&nbsp;we&nbsp;do&nbsp;not&nbsp;log&nbsp;in&nbsp;here&nbsp;but&nbsp;start&nbsp;the&nbsp;verification&nbsp;process</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__keyword">this</span>.VerifyRegistration(model);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__com">//&nbsp;TODO:&nbsp;Notice&nbsp;how&nbsp;we&nbsp;redirect&nbsp;to&nbsp;the&nbsp;confirmation&nbsp;page</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__keyword">return</span>&nbsp;<span class="cs__keyword">this</span>.Json(<span class="cs__keyword">new</span>&nbsp;{&nbsp;success&nbsp;=&nbsp;<span class="cs__keyword">true</span>,&nbsp;redirect&nbsp;=&nbsp;<span class="cs__keyword">this</span>.Url.Action(<span class="cs__string">&quot;Confirmation&quot;</span>)&nbsp;});&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__keyword">this</span>.ModelState.AddModelError(<span class="cs__string">&quot;&quot;</span>,&nbsp;ErrorCodeToString(createStatus));&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;}&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__com">//&nbsp;If&nbsp;we&nbsp;got&nbsp;this&nbsp;far,&nbsp;something&nbsp;failed</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__keyword">return</span>&nbsp;<span class="cs__keyword">this</span>.Json(<span class="cs__keyword">new</span>&nbsp;{&nbsp;errors&nbsp;=&nbsp;<span class="cs__keyword">this</span>.GetErrorsFromModelState()&nbsp;});&nbsp;
}&nbsp;
</pre>
</div>
</div>
</div>
</div>
<h3>Step 2: Sending an Email</h3>
<div>For this step I&rsquo;m going to need an activity that can send email and I want to supply a nicely formatted HTML email with the username embedded and an absolute URL to the Site.css stylesheet.&nbsp; For this example, I decided to create a SendMail activity
 that uses file based email templates.&nbsp; This allows me to treat the body of the HTML mail as content from the site perspective.&nbsp; To improve performance I cache the HTML files after they are read and check to see if the source file has changed before
 using a cached copy.</div>
<blockquote>
<div><em>Using SmtpClient with AsyncCodeActivity was a particular challenge because the SmtpClient class uses an event based async model (EAP) and it took me a while to work out how to use a TaskCompletionSource with AsyncCodeActivity.&nbsp; Take a look at
 the SendMail.cs file for more details.</em></div>
</blockquote>
<div>In the body of the email, I will have to include an absolute URL to the verification page including a verificationCode which is simply the InstanceId of the workflow.&nbsp; Given the enormous amount of data that can apply to an email message, I decided
 to create a type to pass between the MVC code and the Workflow which contains everything I need.</div>
<h5>Problem: HTML Email requires fully qualified URLs</h5>
<div>I want the HTML email to have links which must be fully qualified. I need links to the Site.css file so I can take advantage of styling in the email and the verification URL.&nbsp; To do this, I created the some extension methods to the UrlHelper class</div>
<div>
<div class="scriptcode">
<div class="pluginEditHolder" pluginCommand="mceScriptCode">
<div class="title"><span>C#</span></div>
<div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div>
<span class="hidden">csharp</span>

<div class="preview">
<pre class="csharp"><span class="cs__keyword">public</span>&nbsp;<span class="cs__keyword">static</span>&nbsp;<span class="cs__keyword">string</span>&nbsp;FullyQualifiedAction(<span class="cs__keyword">this</span>&nbsp;UrlHelper&nbsp;urlHelper,&nbsp;<span class="cs__keyword">string</span>&nbsp;actionName,&nbsp;<span class="cs__keyword">string</span>&nbsp;controllerName)&nbsp;
{&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__keyword">return</span>&nbsp;FullyQualify(&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;urlHelper.RequestContext.HttpContext.Request.Url,&nbsp;urlHelper.Action(actionName,&nbsp;controllerName));&nbsp;
}</pre>
</div>
</div>
</div>
</div>
<div>Now when I want to get the fully qualified URL it is very simple</div>
<div>
<div class="scriptcode">
<div class="pluginEditHolder" pluginCommand="mceScriptCode">
<div class="title"><span>C#</span></div>
<div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div>
<span class="hidden">csharp</span>

<div class="preview">
<pre class="csharp"><span class="cs__com">//&nbsp;Created&nbsp;extension&nbsp;methods&nbsp;to&nbsp;provide&nbsp;fully&nbsp;qualified&nbsp;URLs&nbsp;for&nbsp;email</span>&nbsp;
VerificationUrl&nbsp;=&nbsp;<span class="cs__keyword">this</span>.Url.FullyQualifiedAction(<span class="cs__string">&quot;Verification&quot;</span>),&nbsp;
CancelUrl&nbsp;=&nbsp;<span class="cs__keyword">this</span>.Url.FullyQualifiedAction(<span class="cs__string">&quot;Cancel&quot;</span>),&nbsp;
StylesUrl&nbsp;=&nbsp;<span class="cs__keyword">this</span>.Url.FullyQualifiedContent(<span class="cs__string">&quot;~/Content/Site.css&quot;</span>),&nbsp;
</pre>
</div>
</div>
</div>
<h5 class="endscriptcode">Problem: How to merge arguments into the HTML email</h5>
<div class="endscriptcode">In the HTML email I want to merge two kinds of arguments.&nbsp; Some are supplied by the calling code in the BodyArguments array and some are generated automatically.&nbsp; The automatically generated elements can be referred to
 by name.</div>
<div class="endscriptcode"></div>
<div class="endscriptcode">
<div class="scriptcode">
<div class="pluginEditHolder" pluginCommand="mceScriptCode">
<div class="title"><span>HTML</span></div>
<div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div>
<span class="hidden">html</span>

<div class="preview">
<pre class="html"><span class="html__doctype">&lt;!DOCTYPE&nbsp;html&gt;</span>&nbsp;
<span class="html__tag_start">&lt;html</span>&nbsp;<span class="html__attr_name">xmlns</span>=<span class="html__attr_value">&quot;http://www.w3.org/1999/xhtml&quot;</span><span class="html__tag_start">&gt;&nbsp;
</span><span class="html__tag_start">&lt;head</span><span class="html__tag_start">&gt;&nbsp;
</span>&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_start">&lt;title</span><span class="html__tag_start">&gt;</span>Thanks&nbsp;for&nbsp;Registering<span class="html__tag_end">&lt;/title&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_start">&lt;link</span>&nbsp;<span class="html__attr_name">rel</span>=<span class="html__attr_value">&quot;stylesheet&quot;</span>&nbsp;<span class="html__attr_name">type</span>=<span class="html__attr_value">&quot;text/css&quot;</span>&nbsp;<span class="html__attr_name">href</span>=<span class="html__attr_value">&quot;{{StylesUrl}}&quot;</span>&nbsp;<span class="html__tag_start">/&gt;</span>&nbsp;
<span class="html__tag_end">&lt;/head&gt;</span>&nbsp;
<span class="html__tag_start">&lt;body</span><span class="html__tag_start">&gt;&nbsp;
</span>&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_start">&lt;div</span>&nbsp;<span class="html__attr_name">class</span>=<span class="html__attr_value">&quot;featured&quot;</span><span class="html__tag_start">&gt;&nbsp;
</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_start">&lt;hgroup</span>&nbsp;<span class="html__attr_name">class</span>=<span class="html__attr_value">&quot;title&quot;</span><span class="html__tag_start">&gt;&nbsp;
</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_start">&lt;h1</span><span class="html__tag_start">&gt;</span>All&nbsp;most&nbsp;finished...<span class="html__tag_end">&lt;/h1&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_end">&lt;/hgroup&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_start">&lt;p</span><span class="html__tag_start">&gt;&nbsp;
</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Thanks&nbsp;for&nbsp;registering&nbsp;with&nbsp;us&nbsp;{0},&nbsp;Please&nbsp;complete&nbsp;your&nbsp;registration&nbsp;by&nbsp;clicking&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_start">&lt;a</span>&nbsp;<span class="html__attr_name">href</span>=<span class="html__attr_value">&quot;{{VerificationUrl}}&quot;</span><span class="html__tag_start">&gt;</span>here<span class="html__tag_end">&lt;/a&gt;</span>.&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;To&nbsp;cancel&nbsp;your&nbsp;registration,&nbsp;click&nbsp;<span class="html__tag_start">&lt;a</span>&nbsp;<span class="html__attr_name">href</span>=<span class="html__attr_value">&quot;{{CancelUrl}}&quot;</span><span class="html__tag_start">&gt;</span>here<span class="html__tag_end">&lt;/a&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_end">&lt;/p&gt;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="html__tag_end">&lt;/div&gt;</span>&nbsp;
<span class="html__tag_end">&lt;/body&gt;</span>&nbsp;
<span class="html__tag_end">&lt;/html&gt;</span>&nbsp;
</pre>
</div>
</div>
</div>
<div class="endscriptcode">&nbsp;</div>
</div>
<div class="endscriptcode"></div>
<div class="endscriptcode">To keep the SendMail activity very generic, I moved the formatting of the message and merging of the arguments into</div>
<div class="endscriptcode">the FormatMailBody activity.&nbsp; As you can see, when I want to use a generated value in the email such as the stylesheet URL in line 5 I place the keyword inside of double braces.&nbsp; If I want to refer to one of the Body arguments
 that my code created, I just use the typical positional references as in line 13.</div>
</div>
<h3>Step 3: Run the Workflow</h3>
<div>Rather than ask the MVC developer to become an expert on WorkflowApplication, I created a helper class which accepts the Workflow type that you want to use as a template parameter.&nbsp; This allowed me to put in place a simple strongly typed API and hide
 the details of Workflow.&nbsp; For the Workflow, I&rsquo;ve created a StateMachine that does everything I need.&nbsp; Of course, you can make the workflow more complex if you want.&nbsp; I can imagine scenarios where a Human might have to approve membership
 or perhaps there is a membership fee that must be collected, any of these things can be provided for in the StateMachine.</div>
<div><img src="http://i1.code.msdn.s-msft.com/aspnet-mvc4-workflow-c5daa4ab/image/file/57203/1/accountregistration.jpg" alt="" width="379" height="482"></div>
<div></div>
<div>And of course, I&rsquo;ve added support for Debug Tracing of the Workflow as it executes using Microsoft.Activities.Extensions.&nbsp; In the VS Debug window when the Workflow runs you will see nicely formatted trackiing information to help you.</div>
<pre>41: Activity [1.34] &quot;SendMail&quot; scheduled child activity [1.90] &quot;Wait For Confirmation&quot;
42: Activity [1.34] &quot;SendMail&quot; scheduled child activity [1.62] &quot;Sequence&quot;
43: Activity [1.34] &quot;SendMail&quot; scheduled child activity [1.49] &quot;Wait For Resend Command&quot;
44: Activity [1.49] &quot;Wait For Resend Command&quot; is Executing
{
    Arguments
        Command: SendMail
}
45: Activity [1.62] &quot;Sequence&quot; is Executing
46: Activity [1.62] &quot;Sequence&quot; scheduled child activity [1.76] &quot;Delay&quot;
</pre>
<h5>Problem &ndash; How to wait for a command to complete registration</h5>
<div>I like to create an enum that declares the set of commands that I&rsquo;m going to use for my Workflow.&nbsp; In this case, there are a few simple commands.</div>
<div></div>
<div>
<div class="scriptcode">
<div class="pluginEditHolder" pluginCommand="mceScriptCode">
<div class="title"><span>C#</span></div>
<div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div>
<span class="hidden">csharp</span>

<div class="preview">
<pre class="csharp"><span class="cs__keyword">public</span>&nbsp;<span class="cs__keyword">enum</span>&nbsp;RegistrationCommand&nbsp;
{&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;SendMail,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;Confirm,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;Cancel,&nbsp;
}&nbsp;
</pre>
</div>
</div>
</div>
<div class="endscriptcode"></div>
</div>
<div>Then, I used the same technique that I demonstrated in the <a href="http://code.msdn.microsoft.com/windowsdesktop/Windows-Workflow-b4b808a8">
Introduction To StateMachine Hands On Lab</a>.&nbsp; I created an activity which waits for a command using the enum as the bookmark name.</div>
<h5>Problem &ndash; How to monitor workflows with expired timers</h5>
<div>For this example, I have decided against using Windows Server AppFabric because I want to (eventually) run this on Windows Azure.&nbsp; However it is quite simple to plug in the monitoring by launching a thread from the Application_Start method.</div>
<div></div>
<div>
<div class="scriptcode">
<div class="pluginEditHolder" pluginCommand="mceScriptCode">
<div class="title"><span>C#</span></div>
<div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div>
<span class="hidden">csharp</span>

<div class="preview">
<pre class="csharp"><span class="cs__keyword">protected</span>&nbsp;<span class="cs__keyword">void</span>&nbsp;Application_Start()&nbsp;
{&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;AreaRegistration.RegisterAllAreas();&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__com">//&nbsp;Use&nbsp;LocalDB&nbsp;for&nbsp;Entity&nbsp;Framework&nbsp;by&nbsp;default</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;Database.DefaultConnectionFactory&nbsp;=&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__keyword">new</span>&nbsp;SqlConnectionFactory(&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__string">&quot;Data&nbsp;Source=(localdb)\v11.0;&nbsp;Integrated&nbsp;Security=True;&nbsp;MultipleActiveResultSets=True&quot;</span>);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;RegisterGlobalFilters(GlobalFilters.Filters);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;RegisterRoutes(RouteTable.Routes);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="cs__com">//&nbsp;TODO:&nbsp;Notice&nbsp;how&nbsp;we&nbsp;monitor&nbsp;registrations&nbsp;with&nbsp;durable&nbsp;timers</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;RegistrationVerification&lt;AccountRegistration&gt;.MonitorRegistrations();&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;BundleTable.Bundles.RegisterTemplateBundles();&nbsp;
}&nbsp;
</pre>
</div>
</div>
</div>
<h1 class="endscriptcode">&nbsp;Setup</h1>
</div>
<h5>Pre-Requisites</h5>
<div>The sample code requires</div>
<ul>
<li><a href="http://www.microsoft.com/visualstudio/11/en-us/downloads">Visual Studio 11 beta</a>
</li><li><a href="http://www.asp.net/mvc/mvc4">ASP.NET MVC 4</a> </li><li><a href="http://wf.codeplex.com/wikipage?title=Microsoft.Activities%20Overview&referringTitle=Documentation">Microsoft.Activities.Extensions</a> (included)
</li></ul>
<h5>Configuration</h5>
<div>Just look for TODO in the web.config file to find things.</div>
<div>You will first need to create the Workflow Instance Store database.&nbsp; I&rsquo;ve provided some batch files to make things easier</div>
<div><strong>CreateInstanceStore.cmd</strong> &ndash; Drops / Creates the instance store.&nbsp; Close IISExpress prior to running this to close the connection.</div>
<div><strong>Reset.cmd</strong> &ndash; Removes all users from the ASP.NET Membership store, removes / recreates c:\mailbox and re-creates the instance store.&nbsp;</div>
<div>The email is configured to drop messages into c:\mailbox, however you can modify the config to use hotmail or your favorite email provider if you like.</div>
<div>The &lt;appSettings&gt; group includes two values</div>
<div><strong>ReminderDelay</strong> &ndash; The timespan that the workflow will wait before sending a reminder email.&nbsp; For testing you should make this a small value.&nbsp; However keep in mind that after three reminders the account will be deleted so
 if you are debugging you should make this value longer.</div>
<div><strong>InstanceDetectionPeriod</strong> &ndash; The number of seconds that the InstanceStore will wait before polling the database for changes.</div>
<h1>Try It</h1>
<ol>
<li>Press F5 to debug the app </li><li>Register a new user </li><li>Check the C:\Mailbox folder for an email message </li><li>Open the message and click on the confirm link </li><li>The registration will complete </li></ol>
<div>Try other variations like not confirming or trying to log in before you have confirmed etc.</div>
