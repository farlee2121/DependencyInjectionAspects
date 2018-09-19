#

## Primary options
- Option 1: Homebrew
    - Generate an inherited type that runs aspect actions then passes to parent
    - could write a custom provider that creates instantiates these aspect types in place of bound type
    - downsides -> this probably needs reflection and reflection is slow
    - could we generate types at compile time? We have all the info we need at compile time
		- This is basically what PostSharp is, and it's a big undertaking with IL rewritting and such
	- how to generate proxy https://www.codeproject.com/Articles/5511/Dynamic-Proxy-Creation-Using-C-Emit 
- Option 1.1: Windsor or Unity
	- the Windsor DI framework already implemented my DI dynamic proxy idea
	- https://github.com/castleproject/Windsor/blob/master/docs/interceptors.md
	- a more in-depth exploration http://kozmic.net/dynamic-proxy-tutorial/
	- cons: you need to tread carefully around limitations like polymorphism and performance
	- **Unity** also implements interception, but it isn't well documented https://github.com/unitycontainer/unity
- Option 2: Transparent Dynamic Proxy
    - http://www.castleproject.org/projects/dynamicproxy/
    - cons: can only proxy virtual methods

- Option 3: Free IL Rewriters
	- PureSharp
	- https://github.com/Virtuoze/Puresharp
	- Uses Mono.Cecil to rewrite assemblies. Probably more performant, but a bit spooky
	- **Cauldron** is similar https://github.com/reflection-emit/Cauldron
	- Both puresharp and cauldron are active as of august 2018

- Option 4: Give up and realize PostSharp does what I want better than what i've concocted 
    - http://samples.postsharp.net/f/PostSharp.Samples.Authorization/


## Explorations

**Ninject**

where can I hook into ninject for aspects?
	- kernelbase -> readonlykernel
	- pipeline? -> IPipeline
	- binding? -> IBindingResolver

https://github.com/ninject/Ninject/wiki/Providers%2C-Factory-Methods-and-the-Activation-Context
- the providers and activation contexts can be overwritten
    - they run when the object graph is created
    - Once objects are injected, we don't really have a way of hooking into method calls. Ninject is no longer involved

**Other**

Could I secretly package all of my calls as command objects and centrally run shared concerns?
	- Not really, this segments my call chain and is either 1. hands on like before or 2. a proxy problem again

How does asp.net auth work?
- asp.net appears to be able to run filters / middleware because it has control
  over the binding from http to controller methods
  https://github.com/aspnet/Mvc/blob/release/2.2/src/Microsoft.AspNetCore.Mvc.Core/Authorization/AuthorizeFilter.cs
- https://github.com/aspnet/Identity/issues/945
	- Security doesn't seem to actually retrieve the attributes. It consumes the AuthorizeAttribute as IAuthorizeData
	  in AuthorizationPolicy. The only calls to it is the security repo are from tests though
	- MVC
		- Consumes IAuthorize data from several places example (https://github.com/aspnet/Mvc/blob/a67d9363e22be8ef63a1a62539991e1da3a6e30e/src/Microsoft.AspNetCore.Mvc.Core/Internal/AuthorizationApplicationModelProvider.cs)
		- These are already available on context.Result.Controllers where context is a ApplicationModelProviderContext and Controllers is a Ienumberable<ControllerModel>
		- Here is the default implementation where attributes are discovered (via GetCustomAttributes) and placed in a ControllerModel https://github.com/aspnet/Mvc/blob/c16f86f0ef3781e6c86ca9677a3aa8da2266348a/src/Microsoft.AspNetCore.Mvc.Core/Internal/DefaultApplicationModelProvider.cs

How does system.Transaction work ambiently?
https://www.appliedis.com/the-magic-of-transactionscope/
 - Conclusion: this wouldn't help for ambient authorization. It works because the 'resource manager' types
   like SqlClient are aware of the possible tread data for the scope. 
   To utilize a similar paradigm, all of our authed code would have to be wrapped in some kind of context, which
   defeats the goal of preventing intermingled auth code

Would a AuthContext in the style of DbContext be a meaningful advance over if statements?
  - How would it manage failed auth? We'd pretty much be limited to exceptions :/


Chad Mentioned that Unity can operate interceptors on the interfaces independent of their concrete type.
Castle explictly doesn't do with attributes https://github.com/castleproject/Windsor/blob/master/docs/interceptors.md#interceptorattribute
However, it will work if you register interceptors on the interface via DI configuration (and then you don't need virtual methods)

All in all, I would probably want to use a command/strategy pattern with attributes. The more 
logic I can push into the attributes, the more concerns I can re-use without changing my proxy behavior


## Conclusion
Try windsor, it is basically a ready-made version of what I envisioned. If it isn't good enough
then I either go post sharp or drop AOP.

Also create an analogous functional auth scheme to contrast it with AOP design

## Design
- Goal: Gain better understanding of centralized auth paradigms and other shared concerns
- Program: Primary concern is authentication that can be applied consistently across 
  service-level components

- How will I access user information?
	- Ambient context?
	- Attributes don't know their target, so it will have to be available in the
	  interceptor/execution context
	- Can windsor interceptors access context? Yes, at worst I can use IOnBehalfOfAware


