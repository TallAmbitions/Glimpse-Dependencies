Glimpse Dependency Plugin
-------------------------

This is a plugin for [Glimpse][] that shows you the calls made to MVC's Dependency Resolver.

Installation
------------

Install the NuGet [package][].

    PM> install-package glimpse-dependencies

Alternatively you can build the project and place the assembly in your `bin` directory.

Usage
-----

A new tab will appear in your _Glimpse_ console showing you the `IDependencyResolver` call made, the type requested, and the type(s) returned.  Note that MVC caches the results from initial resolution requests and won't ask for the same types.  If you want to view these requests you will need to enable Glimpse on the first request to a fresh server. I plan to update the plugin with a "history" of requests in the future.





[Glimpse]: https://github.com/Glimpse/Glimpse "Glimpse"
[package]: http://nuget.org/List/Packages/glimpse-dependencies "Glimpse-Dependencies NuGet package"