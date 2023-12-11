using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using AdventOfCode;
using AdventOfCode.Commands;

AocCli.WelcomeMessage();
await DotEnv.Load();

return await new CommandLineBuilder(new AocCli())
    .UseDefaults()
    //.UseDependencyInjection(services => services.RegisterOnCourseCliStartup())
    .Build()
    .InvokeAsync(args);
