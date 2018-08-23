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

### Unit Testing

Run a single unit test suite:
```
> dotnet test test/<project>
```

Run all unit test suites:
```
> scripts/unit-test
```

### Feature Testing

Run a single feature test suite:
```
> dotnet test feature/<project>
```

Run all feature test suites:
```
> scripts/feature-test
```

### Install into .NET Global Tools

Install:
```
> dotnet pack
> dotnet tool install --global Steeltoe.Tooling.Cli --add-source src/Steeltoe.Tooling.Cli/bin/Debug 
```

Uninstall:
```
> dotnet tool uninstall --global Steeltoe.Tooling.Cli
```

