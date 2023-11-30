using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using AdventOfCode.Commands;

AocCli.WelcomeMessage();

return await new CommandLineBuilder(new AocCli())
    .UseDefaults()
    //.UseDependencyInjection(services => services.RegisterOnCourseCliStartup())
    .Build()
    .InvokeAsync(args);
