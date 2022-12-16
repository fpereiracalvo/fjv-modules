using Fjv.Modules;

var _args = Environment.GetCommandLineArgs().Skip(1).ToArray();

var factory = new ModuleFactory(typeof(Program).Assembly);

factory.Run(_args);
