# DotLiquid.Mailer

DotLiquid.Mailer is a simple SMTP mailing service which uses 
[DotLiquid](http://dotliquidmarkup.org) as templating engine. DotLiquid is Ruby's 
[Liquid Markup](http://liquidmarkup.org) ported to the .net framework.

All base documentation is available from the 
[DotLiquid Wiki pages](https://github.com/dotliquid/dotliquid/wiki). The test project 
implements some examples on how to use this Mailer plus a small filter collection and 
one custom tag and block taken from the official documentation pages.

This Library integrates 
[viewmodel capability](https://github.com/onybo/DotLiquid-ViewModel) using the LiquidFunctions
created by Olav Nybø. He also wrote a great 
[article](http://blog.novanet.no/generate-html-in-the-backend/) on this.
  
## NuGet

 **Current Version: 1.0.0.0**
 
 Get it from [nuget.org/packages/mailzor](https://nuget.org/packages/mailzor) or via 
 Package Manager Console
 
  > *PM> Install-Package DotLiquid.Mailer*

## Testing

It is recommended to have [smpt4dev](http://smtp4dev.codeplex.com/) running or a real 
mail server configured in order to see the delivery of the test emails.

# Usage

## Initialization
Just instanciate a new MailEngine object (as IMailEngine) and do the apropriate settings.

```csharp
	public IMailEngine dotLiquidMailer;
	
	dotLiquidMailer = new MailEngine()
            {
                DefaultFromAddress = "dotliquid.mailer@mytest.org",
                IsHtml = true,
                SmtpServer = "127.0.0.1",
                SmtpPort = 25,
                UseDefaultCredentials = true,
                TemplateDir = @".\Templates"
            };
			
```

## Singleton
`DotLiquid.Mailer` may also be used as a Singleton, out of the box. All initialization and method 
calls is done by setting the according properties or calling the methods of `MailEngine.Instance`.

```csharp
	MailEngine.Instance.DefaultFromAddress = "dotliquid.mailer@mytest.org";
	MailEngine.Instance.Send( ... );
```

## Send an email based on a file template
Template file names passed to the `SendFromFile` method are extended by the template directory, 
if it is set with the `TemplateDir` property. Here an example, passing data as a viewmodel class.
If the `from` parameter is omitted with any `Send*` metod then an initially set 
`DefaultFromAddress` will be taken.

```csharp
	// preparing the viewmodel
	var orderData = new Order()
    {
        Cust = new Customer()
        {
            Salutation = "Dear Mr. Doe",
            Firstname = "John",
            Lastname = "Doe"
        }
    };

    orderData.Itemlist.AddRange( new List<Item>()
    {
        new Item()
        {
            Name = "Apple",
            Quantity = 5
        },
        new Item()
        {
            Name = "Banana",
            Quantity = 10
        }
    });
	
	// SendFromFile<T>(string subject, string templateFile, T data, string to, string from = "")
	
	// sending the email with explicit from-address
	mailer.SendFromFile<Order>("Your Order", "OrderConfirmation.liquid.html", orderData, 
		"john.doe@nowhere.net", "named.sender@mytest.org")
		
	// sending the email with default from-address
	mailer.SendFromFile<Order>("Your Order", "OrderConfirmation.liquid.html", orderData, 
		"john.doe@nowhere.net")
```

This also works with anonymously created objects as data payload on any `Send*` method.

## Send an email based on a string template from within the code
 
```csharp
	// providing the template as string constant
	public const string template =
        "<!DOCTYPE html>" +
        "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
        "<body>" +
        "{{Cust.Salutation" +
        "    }" +
        "},<br/><br/>" +
        "thank you for the following order made for {{Cust.Firstname}} {{Cust.Lastname}}:" +
        "" +
        "<ul>" +
        "    {% for Item in Itemlist -%}" +
        "    <li>" +
        "        {{Item.Quantity}} x {{Item.Name}}" +
        "    </li>" +
        "    {% endfor -%}" +
        "</ul>" +
        "" +
        "Your Sales Company." +
        "</body>" +
        "</html>";
	
	// preparing data payload (as anonymous object)
	data = new
    {
        Cust = new
        {
            Salutation = "Dear Mr. Doe",
            Firstname = "John",
            Lastname = "Doe"
        },
        ItemList = new[]
        {
            new {Name = "Apple", Quantity = 5},
            new {Name = "Banana", Quantity = 10}
        }
    };
	
	// Send<T>(string subject, string liquidTemplate, T data, string to, string from = "")
	
	// injecting an anonymous object as data payload
	mailer.Send("Your Order", template, _data, "john.doe@nowhere.net", "named.sender@mytest.org");
	
	// or if a default from-address is set:
	mailer.Send("Your Order", template, _data, "john.doe@nowhere.net")
```

## Registering custom Tags and Blocks

As the `MailEngine` partly encapsulates the `DotLiquid` engine it is possible to 
register Tags and Blocks __globally(!)__ using the `RegisterTag` method. So they will 
be available for any MailEngine instance.

```csharp
	// Tag
	mailer.RegisterTag<RandomInt>("random");
	var template ="Your lucky number is {% random 25 %}"
	
	// Block
	mailer.RegisterTag<ByChance>("bychance");
	var template = "{% bychance 20 %}You're the lucky winner!{% endbychance %}";
```

## Registering custom filters

Also custom filters can be registered __globally(!)__ using the `RegisterFilter` method. 

```csharp
	mailer.RegisterFilter(typeof(CaseFilters));
```

Be aware of the naming of a filter when using it in a template. It depends on how you name 
your filter method. For example ... 

```csharp
	var template = "{{'test object' | camelcase }}";
	// if ...
	public static string Camelcase(string input) { ... }
	
	var template = "{{'test object' | camel_case }}"
	// if ...
	public static string CamelCase(string input) { ... }
```

## IoC using Autofac

Here is a description how to register a (pre initialized) MailerEngine with an 
[Autofac](http://autofac.org/) container and how to resolve and use it.

```csharp
	// ---- registration ----

	public IContainer container;

    var builder = new ContainerBuilder();

    builder.Register(c => new MailEngine()
    {
        DefaultFromAddress = "dotliquid.mailer@mytest.org",
        IsHtml = true,
        SmtpServer = "127.0.0.1",
        SmtpPort = 25,
        UseDefaultCredentials = true,
        TemplateDir = @".\Templates"
    })
    .As<IMailEngine>();

    container = builder.Build();

	// ---- usage ----
	
	var mailer = _container.Resolve<IMailEngine>();
	
	// global Tag, Block and Filter registration
	mailer.RegisterTag<RandomInt>("random");
	mailer.RegisterTag<ByChance>("bychance");
	mailer.RegisterFilter(typeof(CaseFilters));

	// setting / sending
	mailer.IsHigh = true;
    mailer.SendFromFile("Your Order", "OrderConfirmation.liquid.html", data, 
		"john.doe@nowhere.net"));
```

## Mailing priotrity

The standard mailing priority is "normal". If an email shall be sent with high priority,
it is needed to set the property `IsHigh` to `true`, __before__ sending the message. When 
the apropriate email is sent it will be reset to "normal", again.

## Using Credentials

The `MailEngine` provides two kinds of credentials to be set by their property: default
credentials and network credentials.

```csharp
	// configuring to use default credentials
	mailer.UseDefaultCredentials = true;
	
	// configuring to use network credentials
	mailer.Credentials = new System.Net.NetworkCredential("myEmailServiceAccount", "Password");
``` 	

## Using SSL

The SSL encryption also can be turned "on" e. g. for seczrely sending a password with 
the credentiasls.

```csharp
	mailer.EnableSsl = true;
```

# Change Log

 Version 1.0.0.0 - Initial release
 
# License
Licensed under the MIT license.