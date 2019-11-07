# CRUD operation with angularJS hot towel template
## Requires
- Visual Studio 2013
## License
- MS-LPL
## Technologies
- Entity Framework
- Bootstrap
- ASP.NET MVC 4
- AngularJS
## Topics
- ASP.NET MVC
- Bootstrap
- Controller in ANgularJS
- Hot towel
## Updated
- 05/14/2015
## Description

<p><span style="font-size:large">Introduction</span></p>
<p><em>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; The utility uses AngularJS hot towel template, MVC and entity frame work. The application uses simple contact list and list is displayed in web UI two different types views one
 with boot strap and other with normal to do the CRUD operation. The current hot towel template does not provide feature to use HTTP and angular controller by default. The utility extended the hot towel template to use HTTP and angular controller.
<br>
</em></p>
<p><em><span><strong><span style="font-size:medium">Building the Sample </span></strong><br>
</span></em></p>
<p><em>The entire source code can be downloaded MVCApplication3.zip.<em>The database mdf file is attached with project.&nbsp;
<br>
</em>the connection string path has to be changed in web.config in order to run the application.</em></p>
<p>&nbsp;</p>
<p><em><span style="font-size:20px; font-weight:bold">Description</span></em></p>
<p><em><span style="font-size:xx-small"><em>The utility uses AngularJS hot towel template, MVC and entity frame work.
</em>The utility is uses the hot towel template to $http and controller. the application shows two different types of views to perform CRUD operation.Also uses angular control to just save the data. The application is the extension of hot towel template which
 would help the developers to implement and incorporate with MVC along with hot towel angular JS template. The entity frame work is used to perform CRUD operation along with angularJS. MVC4 and JSON are used in the application<br>
</span></em></p>
<p>&nbsp;</p>
<p><em><span style="font-size:20px; font-weight:bold"><br>
</span></em></p>
<p><em><img id="130733" src="130733-display.png" alt="" width="1366" height="728"><br>
</em></p>
<p>&nbsp;</p>
<div class="scriptcode">
<div class="pluginEditHolder" pluginCommand="mceScriptCode">
<div class="title"><span>JavaScript</span><span>C#</span><span>HTML</span></div>
<div class="pluginLinkHolder"><span class="pluginEditHolderLink">Edit</span>|<span class="pluginRemoveHolderLink">Remove</span></div>
<span class="hidden">js</span><span class="hidden">csharp</span><span class="hidden">html</span>
<pre class="hidden">(function () {
    'use strict';

    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getRoutes());
    
    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes', routeConfigurator]);
    function routeConfigurator($routeProvider, routes) {

        routes.forEach(function (r) {
            $routeProvider.when(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });
    }

    // Define the routes 
    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/dashboard/dashboard.html',
                    title: 'dashboard',
                    settings: {
                        nav: 1,
                        content: '&lt;i class=&quot;fa fa-dashboard&quot;&gt;&lt;/i&gt; User Dashboard'
                    }
                }
            }, {
                url: '/admin',
                config: {
                    title: 'Contacts',
                    templateUrl: 'app/admin/admin.html',
                    settings: {
                        nav: 2,
                        content: '&lt;i class=&quot;fa fa-lock&quot;&gt;&lt;/i&gt; User Dashboard V.10'
                    }
                }
            },
            {

                url: '/add',
                config: {
                    title: 'Add user',
                    templateUrl: 'app/user/adduser.html',
                    controller: 'addusercontroller'
                 
                }
            }
        ];
    }
})();



(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').factory(serviceId, ['common', datacontext]);

    function datacontext(common) {
        var $q = common.$q;
        var $http = common.$http;
        //var $scope = common.$scope;
        var service = {
            getPeople: getPeople,
            getMessageCount: getMessageCount,
            savePeople: savePeople,
            modifyuser: modifyuser,
            deletePeople:deletePeople
        };

        return service;

        function getMessageCount() { return $q.when(72); }
        function modifyuser(user,$scope)
        {
           // var $scope = $scope;
            var request = $http({
                method: &quot;post&quot;,
                url: &quot;/HotTowel/ModifyPeople&quot;,
                data: user
            });

            return request.then(handleSucess, handleError);
        }

        function deletePeople(user, $scope,idx) {
           // var $scope = $scope;
            var request = $http({
                method: &quot;post&quot;,
                url: &quot;/HotTowel/DeletePeople&quot;,
                data: user
            });

            return request.then(function (response) {
                $scope.vm.people.splice(idx, 1);
                $scope.Modifymessage = &quot;Data deleted successfully&quot;
            },
            function (error) {
                handleError(error);
            }
               );
        }


        function savePeople(user,$scope)
        {
                var request = $http({
                    method: &quot;post&quot;,
                    url: &quot;/HotTowel/SavePeople&quot;,
                    data: user
                });

              //  var nsg = request.then(handleSucess, handleError);

                return request.then(function (response) {
                    $scope.vm.people.push(response.data);
                    $scope.message = &quot;Data saved successfully&quot;
                    $scope.user.firstname = &quot;&quot;;
                    $scope.user.lastname = &quot;&quot;;
                    $scope.user.age = &quot;&quot;;
                    $scope.user.location = &quot;&quot;;
                },
                function (error) {
                    handleError(Error);
                }
                );
                    //handleSucess,handleError);
        }
        function handleSucess(response)
        {
            
            return response.data;
        }
        function handleError(response)
        {
            // The API response from the server should be returned in a
            // nomralized format. However, if the request was not handled by the
            // server (or what not handles properly - ex. server error), then we
            // may have to normalize it on our end, as best we can.
            if (
            !angular.isObject(response.data) ||
            !response.data.message
            ) {
                return ($q.reject(&quot;An unknown error occurred.&quot;));
            }
            // Otherwise, use expected error message.
            return ($q.reject(response.data.message));

        }

        function getPeople() {

            // Get the deferred object
            var deferred = $q.defer();
            // Initiates the AJAX call
            $http({ method: 'GET', url: '/HotTowel/GetPeopleDetails' }).success(deferred.resolve).error(deferred.reject);
            // Returns the promise - Contains result once request completes
            return deferred.promise;
            /*
            var people = [
                { firstName: 'John', lastName: 'Papa', age: 25, location: 'Florida' },
                { firstName: 'Ward', lastName: 'Bell', age: 31, location: 'California' },
                { firstName: 'Colleen', lastName: 'Jones', age: 21, location: 'New York' },
                { firstName: 'Madelyn', lastName: 'Green', age: 18, location: 'North Dakota' },
                { firstName: 'Ella', lastName: 'Jobs', age: 18, location: 'South Dakota' },
                { firstName: 'Landon', lastName: 'Gates', age: 11, location: 'South Carolina' },
                { firstName: 'Haley', lastName: 'Guthrie', age: 35, location: 'Wyoming' }
            ];
            return $q.when(people);
            */
        }
    }
})();</pre>
<pre class="hidden"> public class HotTowelController : Controller
    {
        //
        // GET: /HotTowel/
        PeopleRepository  Repository = new PeopleRepository();
 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ModifyPeople(Contacts people)
        {
            Repository.ModifyUser(people);
            return new EmptyResult();
        }
        public ActionResult SavePeople(Contacts people)
        {
            /*
            var contacts = from b in Repository.Contacts
                           where b.FirstName == people.FirstName
                           select b;
            int count = contacts.Count&lt;Contacts&gt;();

                if (count &gt; 0)
                    return new HttpStatusCodeResult(404,&quot;Name already present&quot;);
             * 
             */
            Repository.Save(people);
            return getJson&lt;Contacts&gt;(people);
        }


        public ActionResult DeletePeople(Contacts people)
        {
            Repository.Delete(people);

            return new EmptyResult();
        }
        public ActionResult GetPeopleDetails()
        {


            return getJson&lt;DbSet&gt;(Repository.Contacts);

        }

        private ActionResult getJson&lt;TEnitry&gt;(TEnitry obj)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            var jsonResult = new ContentResult
            {
                Content = JsonConvert.SerializeObject(obj, settings),
                ContentType = &quot;application/json&quot;

            };


            return jsonResult;
        }
    }


public class PeopleRepository : DbContext
    {
        public PeopleRepository() : base(&quot;name=ContactsEntities&quot;) { }
        //static List&lt;Contacts&gt; listPeople;
        

        public System.Data.Entity.DbSet&lt;Contacts&gt; Contacts { get; set; }

        public void Save(Contacts people)
        {
            
            Contacts.Add(people);
           // Configuration.ValidateOnSaveEnabled = false;
            SaveChanges();
        }

        public void ModifyUser(Contacts people)
        {
            var original = Contacts.Find(people.id);
            if (original != null)
            {
                Entry(original).CurrentValues.SetValues(people);// State = EntityState.Modified;
                // Contacts.Attach(people);
                SaveChanges();
            }
        }

        internal void Delete(Contacts people)
        {
            var user= from x in this.Contacts where x.id == people.id select x;
            this.Contacts.Remove(user.First&lt;Contacts&gt;());
            SaveChanges();
        }
    }</pre>
<pre class="hidden">&lt;section class=&quot;mainbar&quot; data-ng-controller=&quot;admin as vm&quot;&gt;
    &lt;section class=&quot;matter&quot;&gt;
        &lt;div class=&quot;container&quot;&gt;
            &lt;div class=&quot;row&quot;&gt;
                &lt;div class=&quot;widget wviolet col-lg-8&quot;&gt;
                    &lt;div data-cc-widget-header title=&quot;{{vm.title}}&quot;  allow-collapse=&quot;true&quot;&gt;&lt;/div&gt;
                    &lt;div class=&quot;widget-content user&quot;&gt;
                        &lt;p&gt;&lt;a data-ng-click=&quot;showadd()&quot; href=&quot;javascript:;&quot; class=&quot;btn btn-primary&quot;&gt;Add New User&lt;/a&gt;&lt;/p&gt;
                        &lt;div class=&quot;modal fade&quot; id=&quot;userModel&quot; tabindex=&quot;-1&quot; role=&quot;dialog&quot; aria-hidden=&quot;true&quot;&gt;
                            &lt;div class=&quot;modal-dialog&quot;&gt;
                                &lt;div class=&quot;modal-content&quot;&gt;
                                    &lt;div class=&quot;modal-header&quot;&gt;
                                        &lt;button type=&quot;button&quot; class=&quot;close bgreen&quot; data-dismiss=&quot;modal&quot;  aria-hidden=&quot;true&quot;&gt;X&lt;/button&gt;
                                        &lt;h4 class=&quot;modal-title bblue&quot; id=&quot;myModalLabel&quot; ng-hide=&quot;editMode&quot;&gt;Add &lt;/h4&gt;
                                        &lt;h4 class=&quot;modal-title bblue&quot; id=&quot;myModalLabel&quot; ng-show=&quot;editMode&quot;&gt;Edit&lt;/h4&gt;
                                    &lt;/div&gt;
                                    &lt;div class=&quot;modal-body&quot;&gt;
                                       &lt;form class=&quot;form-horizontal&quot; role=&quot;form&quot; name=&quot;adduserform&quot;&gt;


                                            &lt;div class=&quot;form-group left&quot;&gt;

                                                &lt;label for=&quot;title&quot; class=&quot;col-sm-5 control-label&quot;&gt;First Name&lt;/label&gt;
                                                &lt;div class=&quot;col-sm-10&quot;&gt;
                                                    &lt;input type=&quot;text&quot; data-ng-model=&quot;user.firstname&quot; class=&quot;form-control&quot; id=&quot;title&quot; placeholder=&quot;Your First Name&quot; required title=&quot;Enter your name&quot; /&gt;
                                                &lt;/div&gt;
                                            &lt;/div&gt;
                                            &lt;div class=&quot;form-group&quot;&gt;
                                                &lt;label for=&quot;title&quot; class=&quot;col-sm-10 control-label&quot;&gt;Last Name&lt;/label&gt;
                                                &lt;div class=&quot;col-sm-10&quot;&gt;
                                                    &lt;input type=&quot;text&quot; data-ng-model=&quot;user.lastname&quot; class=&quot;form-control&quot; id=&quot;title&quot; placeholder=&quot;Your Last Name&quot; required title=&quot;Enter your last name&quot; /&gt;
                                                &lt;/div&gt;
                                            &lt;/div&gt;
                                            &lt;div class=&quot;form-group&quot;&gt;
                                                &lt;label for=&quot;title&quot; class=&quot;col-sm-10 control-label&quot;&gt;Age&lt;/label&gt;
                                                &lt;div class=&quot;col-sm-10&quot;&gt;
                                                    &lt;input type=&quot;text&quot; data-ng-model=&quot;user.age&quot; class=&quot;form-control&quot; id=&quot;title&quot; placeholder=&quot;Age&quot; required title=&quot;Enter your Age&quot; /&gt;
                                                &lt;/div&gt;
                                            &lt;/div&gt;
                                            &lt;div class=&quot;form-group&quot;&gt;
                                                &lt;label for=&quot;title&quot; class=&quot;col-sm-10 control-label&quot;&gt;Location&lt;/label&gt;
                                                &lt;div class=&quot;col-sm-10&quot;&gt;
                                                    &lt;input type=&quot;text&quot; data-ng-model=&quot;user.location&quot; class=&quot;form-control&quot; id=&quot;title&quot; placeholder=&quot;Your Location&quot; required title=&quot;Enter your location&quot; /&gt;
                                                &lt;/div&gt;
                                            &lt;/div&gt;
                                            &lt;div class=&quot;form-group&quot;&gt;
                                                &lt;div class=&quot;col-sm-offset-2 col-sm-10&quot;&gt;
                                                    &lt;span data-ng-hide=&quot;editMode&quot;&gt;
                                                        &lt;input type=&quot;submit&quot; value=&quot;Add&quot; data-ng-click=&quot;add()&quot; class=&quot;btn btn-primary&quot; /&gt;
                                                    &lt;/span&gt;
                                                    &lt;span data-ng-show=&quot;editMode&quot;&gt;
                                                        &lt;input type=&quot;submit&quot; value=&quot;Update&quot; data-ng-click=&quot;update(user)&quot; class=&quot;btn btn-primary&quot; /&gt;
                                                    &lt;/span&gt;
                                                    &lt;input type=&quot;button&quot; value=&quot;Cancel&quot; data-ng-click=&quot;cancel()&quot; class=&quot;btn btn-primary&quot; /&gt;
                                                &lt;/div&gt;
                                            &lt;/div&gt;
                                        &lt;/form&gt;
                                    &lt;/div&gt;
                                &lt;/div&gt;
                            &lt;/div&gt;
                        &lt;/div&gt;
                    
                    &lt;strong class=&quot;success center-block green&quot;&gt;{{message}}&lt;/strong&gt;
                    &lt;table class=&quot;table table-hover&quot;&gt;
                        &lt;thead&gt;
                            &lt;tr&gt;

                                &lt;th&gt;First Name&lt;/th&gt;
                                &lt;th&gt;Last Name&lt;/th&gt;
                                &lt;th&gt;Age&lt;/th&gt;
                                &lt;th&gt;Location&lt;/th&gt;
                                &lt;th&gt;Edit/Delete&lt;/th&gt;
                            &lt;/tr&gt;
                        &lt;/thead&gt;
                        &lt;tbody&gt;
                            &lt;tr data-ng-repeat=&quot;p in vm.people | filter:name&quot;&gt;
                                &lt;td&gt;&lt;strong&gt;{{p.firstname}}&lt;/strong&gt; &lt;/td&gt;
                                &lt;td&gt;&lt;strong&gt;{{p.lastname}} &lt;/strong&gt;&lt;/td&gt;
                                &lt;td&gt;&lt;strong&gt;{{p.age}}&lt;/strong&gt; &lt;/td&gt;
                                &lt;td&gt;&lt;strong&gt;{{p.location}} &lt;/strong&gt; &lt;/td&gt;
                                &lt;td&gt;
                                    &lt;input type=&quot;button&quot; ng-click=&quot;showedit(p)&quot; class=&quot;btn btn-primary&quot; value=&quot;Edit&quot; /&gt;
                                    &lt;input type=&quot;button&quot; ng-click=&quot;delete($index)&quot; class=&quot;btn btn-primary&quot; value=&quot;Delete&quot; /&gt;
                                &lt;/td&gt;
                            &lt;/tr&gt;
                        &lt;/tbody&gt;
                    &lt;/table&gt;
                    &lt;div class=&quot;widget-foot&quot;&gt;
                        &lt;div class=&quot;clearfix&quot;&gt;&lt;/div&gt;
                    &lt;/div&gt;
                &lt;/div&gt;
            &lt;/div&gt;
        &lt;/div&gt;
        &lt;/div&gt;
    &lt;/section&gt;
&lt;/section&gt;
</pre>
<div class="preview">
<pre class="js">(<span class="js__operator">function</span>&nbsp;()&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__string">'use&nbsp;strict'</span>;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;app&nbsp;=&nbsp;angular.module(<span class="js__string">'app'</span>);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;Collect&nbsp;the&nbsp;routes</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;app.constant(<span class="js__string">'routes'</span>,&nbsp;getRoutes());&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;Configure&nbsp;the&nbsp;routes&nbsp;and&nbsp;route&nbsp;resolvers</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;app.config([<span class="js__string">'$routeProvider'</span>,&nbsp;<span class="js__string">'routes'</span>,&nbsp;routeConfigurator]);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;routeConfigurator($routeProvider,&nbsp;routes)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;routes.forEach(<span class="js__operator">function</span>&nbsp;(r)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$routeProvider.when(r.url,&nbsp;r.config);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$routeProvider.otherwise(<span class="js__brace">{</span>&nbsp;redirectTo:&nbsp;<span class="js__string">'/'</span>&nbsp;<span class="js__brace">}</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;Define&nbsp;the&nbsp;routes&nbsp;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;getRoutes()&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;[&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;url:&nbsp;<span class="js__string">'/'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;config:&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;templateUrl:&nbsp;<span class="js__string">'app/dashboard/dashboard.html'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;title:&nbsp;<span class="js__string">'dashboard'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;settings:&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;nav:&nbsp;<span class="js__num">1</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;content:&nbsp;<span class="js__string">'&lt;i&nbsp;class=&quot;fa&nbsp;fa-dashboard&quot;&gt;&lt;/i&gt;&nbsp;User&nbsp;Dashboard'</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>,&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;url:&nbsp;<span class="js__string">'/admin'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;config:&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;title:&nbsp;<span class="js__string">'Contacts'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;templateUrl:&nbsp;<span class="js__string">'app/admin/admin.html'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;settings:&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;nav:&nbsp;<span class="js__num">2</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;content:&nbsp;<span class="js__string">'&lt;i&nbsp;class=&quot;fa&nbsp;fa-lock&quot;&gt;&lt;/i&gt;&nbsp;User&nbsp;Dashboard&nbsp;V.10'</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;url:&nbsp;<span class="js__string">'/add'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;config:&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;title:&nbsp;<span class="js__string">'Add&nbsp;user'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;templateUrl:&nbsp;<span class="js__string">'app/user/adduser.html'</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;controller:&nbsp;<span class="js__string">'addusercontroller'</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;];&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
<span class="js__brace">}</span>)();&nbsp;
&nbsp;
&nbsp;
&nbsp;
(<span class="js__operator">function</span>&nbsp;()&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__string">'use&nbsp;strict'</span>;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;serviceId&nbsp;=&nbsp;<span class="js__string">'datacontext'</span>;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;angular.module(<span class="js__string">'app'</span>).factory(serviceId,&nbsp;[<span class="js__string">'common'</span>,&nbsp;datacontext]);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;datacontext(common)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;$q&nbsp;=&nbsp;common.$q;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;$http&nbsp;=&nbsp;common.$http;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//var&nbsp;$scope&nbsp;=&nbsp;common.$scope;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;service&nbsp;=&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;getPeople:&nbsp;getPeople,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;getMessageCount:&nbsp;getMessageCount,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;savePeople:&nbsp;savePeople,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;modifyuser:&nbsp;modifyuser,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;deletePeople:deletePeople&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;service;&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;getMessageCount()&nbsp;<span class="js__brace">{</span>&nbsp;<span class="js__statement">return</span>&nbsp;$q.when(<span class="js__num">72</span>);&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;modifyuser(user,$scope)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;var&nbsp;$scope&nbsp;=&nbsp;$scope;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;request&nbsp;=&nbsp;$http(<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;method:&nbsp;<span class="js__string">&quot;post&quot;</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;url:&nbsp;<span class="js__string">&quot;/HotTowel/ModifyPeople&quot;</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;data:&nbsp;user&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;request.then(handleSucess,&nbsp;handleError);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;deletePeople(user,&nbsp;$scope,idx)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;var&nbsp;$scope&nbsp;=&nbsp;$scope;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;request&nbsp;=&nbsp;$http(<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;method:&nbsp;<span class="js__string">&quot;post&quot;</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;url:&nbsp;<span class="js__string">&quot;/HotTowel/DeletePeople&quot;</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;data:&nbsp;user&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;request.then(<span class="js__operator">function</span>&nbsp;(response)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$scope.vm.people.splice(idx,&nbsp;<span class="js__num">1</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$scope.Modifymessage&nbsp;=&nbsp;<span class="js__string">&quot;Data&nbsp;deleted&nbsp;successfully&quot;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;(error)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;handleError(error);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;savePeople(user,$scope)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;request&nbsp;=&nbsp;$http(<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;method:&nbsp;<span class="js__string">&quot;post&quot;</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;url:&nbsp;<span class="js__string">&quot;/HotTowel/SavePeople&quot;</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;data:&nbsp;user&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>);&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;&nbsp;var&nbsp;nsg&nbsp;=&nbsp;request.then(handleSucess,&nbsp;handleError);</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;request.then(<span class="js__operator">function</span>&nbsp;(response)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$scope.vm.people.push(response.data);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$scope.message&nbsp;=&nbsp;<span class="js__string">&quot;Data&nbsp;saved&nbsp;successfully&quot;</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$scope.user.firstname&nbsp;=&nbsp;<span class="js__string">&quot;&quot;</span>;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$scope.user.lastname&nbsp;=&nbsp;<span class="js__string">&quot;&quot;</span>;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$scope.user.age&nbsp;=&nbsp;<span class="js__string">&quot;&quot;</span>;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$scope.user.location&nbsp;=&nbsp;<span class="js__string">&quot;&quot;</span>;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>,&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;(error)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;handleError(<span class="js__error">Error</span>);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//handleSucess,handleError);</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;handleSucess(response)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;response.data;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;handleError(response)&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;The&nbsp;API&nbsp;response&nbsp;from&nbsp;the&nbsp;server&nbsp;should&nbsp;be&nbsp;returned&nbsp;in&nbsp;a</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;nomralized&nbsp;format.&nbsp;However,&nbsp;if&nbsp;the&nbsp;request&nbsp;was&nbsp;not&nbsp;handled&nbsp;by&nbsp;the</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;server&nbsp;(or&nbsp;what&nbsp;not&nbsp;handles&nbsp;properly&nbsp;-&nbsp;ex.&nbsp;server&nbsp;error),&nbsp;then&nbsp;we</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;may&nbsp;have&nbsp;to&nbsp;normalize&nbsp;it&nbsp;on&nbsp;our&nbsp;end,&nbsp;as&nbsp;best&nbsp;we&nbsp;can.</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">if</span>&nbsp;(&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!angular.isObject(response.data)&nbsp;||&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;!response.data.message&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;($q.reject(<span class="js__string">&quot;An&nbsp;unknown&nbsp;error&nbsp;occurred.&quot;</span>));&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;Otherwise,&nbsp;use&nbsp;expected&nbsp;error&nbsp;message.</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;($q.reject(response.data.message));&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__operator">function</span>&nbsp;getPeople()&nbsp;<span class="js__brace">{</span>&nbsp;
&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;Get&nbsp;the&nbsp;deferred&nbsp;object</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">var</span>&nbsp;deferred&nbsp;=&nbsp;$q.defer();&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;Initiates&nbsp;the&nbsp;AJAX&nbsp;call</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$http(<span class="js__brace">{</span>&nbsp;method:&nbsp;<span class="js__string">'GET'</span>,&nbsp;url:&nbsp;<span class="js__string">'/HotTowel/GetPeopleDetails'</span>&nbsp;<span class="js__brace">}</span>).success(deferred.resolve).error(deferred.reject);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__sl_comment">//&nbsp;Returns&nbsp;the&nbsp;promise&nbsp;-&nbsp;Contains&nbsp;result&nbsp;once&nbsp;request&nbsp;completes</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__statement">return</span>&nbsp;deferred.promise;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__ml_comment">/*&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;var&nbsp;people&nbsp;=&nbsp;[&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;firstName:&nbsp;'John',&nbsp;lastName:&nbsp;'Papa',&nbsp;age:&nbsp;25,&nbsp;location:&nbsp;'Florida'&nbsp;},&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;firstName:&nbsp;'Ward',&nbsp;lastName:&nbsp;'Bell',&nbsp;age:&nbsp;31,&nbsp;location:&nbsp;'California'&nbsp;},&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;firstName:&nbsp;'Colleen',&nbsp;lastName:&nbsp;'Jones',&nbsp;age:&nbsp;21,&nbsp;location:&nbsp;'New&nbsp;York'&nbsp;},&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;firstName:&nbsp;'Madelyn',&nbsp;lastName:&nbsp;'Green',&nbsp;age:&nbsp;18,&nbsp;location:&nbsp;'North&nbsp;Dakota'&nbsp;},&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;firstName:&nbsp;'Ella',&nbsp;lastName:&nbsp;'Jobs',&nbsp;age:&nbsp;18,&nbsp;location:&nbsp;'South&nbsp;Dakota'&nbsp;},&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;firstName:&nbsp;'Landon',&nbsp;lastName:&nbsp;'Gates',&nbsp;age:&nbsp;11,&nbsp;location:&nbsp;'South&nbsp;Carolina'&nbsp;},&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;firstName:&nbsp;'Haley',&nbsp;lastName:&nbsp;'Guthrie',&nbsp;age:&nbsp;35,&nbsp;location:&nbsp;'Wyoming'&nbsp;}&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;];&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return&nbsp;$q.when(people);&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*/</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;<span class="js__brace">}</span>&nbsp;
<span class="js__brace">}</span>)();</pre>
</div>
</div>
</div>
<h1><span>Source Code Files</span></h1>
<ul>
<li><em>source code file name #1 - summary for this source code file.</em> </li><li><em><em>source code file name #2 - summary for this source code file.</em></em>
</li></ul>
<h1>More Information</h1>
<p><em>For more information on X, see ...?</em></p>