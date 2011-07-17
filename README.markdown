Glimpse Dependency Plugin
=========================

This is a plugin for [Glimpse][] that shows you the calls made to MVC's Dependency Resolver.

![Screenshot][]

Installation
------------

Install the NuGet [package][].

    PM> install-package glimpse-dependencies

Alternatively you can build the project and place the assembly in your `bin` directory.

Usage
-----

A new tab will appear in your _Glimpse_ console showing you the `IDependencyResolver` call made, the type requested, and the type(s) returned.  Note that MVC caches the results from initial resolution requests and won't ask for the same types again.  The plugin displays types requested in an earlier request as grey.  New requests for the same type supplant the previous types. Only resolution requests made while Glimpse is enabled are tracked.

Changes
-------

    1.0: First version
    1.1: Add resolution history.
    1.2: Update to work with Glimpse 0.82.
    1.3: Update to work with Glimpse 0.83.

[Glimpse]: https://github.com/Glimpse/Glimpse "Glimpse"
[package]: http://nuget.org/List/Packages/glimpse-dependencies "Glimpse-Dependencies NuGet package"
[Screenshot]: https://github.com/TallAmbitions/Glimpse-Dependencies/raw/master/assets/glimpse-dependency-screenshot.png "Plugin Screenshot"