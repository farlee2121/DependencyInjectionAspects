# Primary options
- Option 1: Generate an inherited type that runs aspect actions then passes to parent
    - could write a custom provider that creates instantiates these aspect types in place of bound type
    - downsides -> this probably needs reflection and reflection is slow
    - could we generate types at compile time? We have all the info we need at compile time
		- This is basically what PostSharp is, and it's a big undertaking with IL rewritting and such
	- how to generate proxy https://www.codeproject.com/Articles/5511/Dynamic-Proxy-Creation-Using-C-Emit 
- Option 1.1 Windsor
	- the Windsor DI framework already implemented my DI dynamic proxy idea
	- https://github.com/castleproject/Windsor/blob/master/docs/interceptors.md
	- cons: you need to tread carefully around limitations like polymorphism and performance

- Option 2: Transparent Dynamic Proxy
    - http://www.castleproject.org/projects/dynamicproxy/
    - cons: can only proxy virtual methods

- Option 3: Give up and realize PostSharp does what I want better than what i've concocted 
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

All in all, I would probably want to use a command/strategy pattern with attributes. The more 
logic I can push into the attributes, the more concerns I can re-use without changing my proxy behavior

## Conclusion
Try windsor, it is basically a ready-made version of what I envisioned. If it isn't good enough
then I either go post sharp or drop AOP.

Also create an analogous functional auth scheme to contrast the two

## Design
- How will I access user information?
	- Ambient context?
	- Attributes don't know their target, so it will have to be available in the
	  interceptor/execution context
	- Can windsor interceptors access context? Yes, at worst I can use IOnBehalfOfAware


