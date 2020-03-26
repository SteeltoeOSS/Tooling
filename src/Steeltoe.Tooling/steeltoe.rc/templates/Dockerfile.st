FROM <image>
WORKDIR /<project.Name>
RUN mkdir -p /usr/local/share/dotnet/sdk/NuGetFallbackFolder
CMD ["dotnet", "watch", "run", "--urls", "<project.Protocols:{protocol |<protocol.Name>://0.0.0.0:<protocol.Port>}; separator=";">"]
