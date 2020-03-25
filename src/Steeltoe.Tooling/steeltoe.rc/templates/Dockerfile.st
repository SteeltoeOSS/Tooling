FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /<project.Name>
RUN mkdir -p /usr/local/share/dotnet/sdk/NuGetFallbackFolder
CMD ["dotnet", "watch", "run", "--urls", "<project.Services:{svc|<svc.Protocol>://0.0.0.0:<svc.Port>}; separator=";">"]
