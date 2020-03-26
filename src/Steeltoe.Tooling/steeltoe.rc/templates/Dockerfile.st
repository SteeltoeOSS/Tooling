FROM <image>
WORKDIR /<project.Name>
RUN mkdir -p /usr/local/share/dotnet/sdk/NuGetFallbackFolder
CMD ["dotnet", "watch", "run", "--urls", "<project.Services:{svc |<svc.Protocol>://0.0.0.0:<svc.Port>}; separator=";">"]
