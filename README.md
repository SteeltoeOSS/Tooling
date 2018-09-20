# Steeltoe Tooling

Steeltoe developer tool collection.

For documentation, please see the [Steeltoe Tooling Wiki](https://github.com/SteeltoeOSS/Tooling/wiki).

## Latest Builds

Platform | Branch | Status
-------- | ------ | ------
Windows  | master | [![AppVeyor master status](https://ci.appveyor.com/api/projects/status/bpwhsnue8j7iiwpp/branch/master?svg=true)](https://ci.appveyor.com/project/steeltoe/tooling/branch/master)
Windows  | dev | [![AppVeyor dev status](https://ci.appveyor.com/api/projects/status/bpwhsnue8j7iiwpp/branch/dev?svg=true)](https://ci.appveyor.com/project/steeltoe/tooling/branch/dev)
Linux/OS X  | master | [![Travis CI master status](https://travis-ci.org/SteeltoeOSS/Tooling.svg?branch=master)](https://travis-ci.org/SteeltoeOSS/Tooling)
Linux/OS X  | dev | [![Travis CI dev status](https://travis-ci.org/SteeltoeOSS/Tooling.svg?branch=dev)](https://travis-ci.org/SteeltoeOSS/Tooling)

## Developer Chores

### Testing

Run unit test suites:
```
> scripts/unit-test

# Alternatively
> dotnet test test/Steeltoe.Tooling.Test
```

Run CLI test suites (takes several minutes):
```
> scripts/cli-test

# Alternatively
> dotnet test test/Steeltoe.Cli.Test
```

### Install into .NET Global Tools

Install:
```
> scripts/install

# Alternatively
> dotnet pack
> dotnet tool install -g --add-source src/Steeltoe.Cli/bin/Debug Steeltoe.Cli
```

Uninstall:
```
> dotnet tool uninstall -g Steeltoe.Cli
```

