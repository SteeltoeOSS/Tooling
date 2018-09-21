# SpringOne Steeltoe Class

Install Steeltoe Tooling CLI.

```
$ dotnet tool install -g --add-source https://www.myget.org/F/steeltoedev/api/v3/index.json steeltoe.cli --version 0.0.1-springone-2018-00092
```

Open a terminal in this directory.

_Example:_
```
$ git clone https://github.com/SteeltoeOSS/Tooling.git SpringOneSteeltoe
$ cd SpringOneSteeltoe
$ git checkout springone-2018
$ cd class
```

Start required services:
```
$ steeltoe deploy
```

Stop required services:
```
$ steeltoe undeploy
```

Service statuses:
```
$ steeltoe status
```

