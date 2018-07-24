# Steeltoe Developer Tools

## Install/Uninstall into Global Tools

Install:
```
> dotnet pack --output ./
> dotnet tool install -g steeltoe --add-source ./
```

After installing, you can run the `steeltoe` command from a commandline:
```
> steeltoe -h
Steeltoe Developer Tools

Usage: steeltoe [options] [command]

Options:
  -?|-h|--help  Show help information

Commands:
  doctor        Health check for your Steeltoe development environment

Run 'steeltoe [command] --help' for more information about a command.
```

Uninstall:
```
> dotnet tool uninstall -g steeltoe
```


## Running from within this Project

Alternatively, you can run directly from within the project without having to install into Global Tools.

Example: Run with no command or options
```
> dotnet run --project steeltoe
```

Example: Run a command
```
> dotnet run --project steeltoe doctor
```

Example: Run a command options (_note "`--`" delimiter_)
```
> dotnet run --project steeltoe -- doctor -h
```

