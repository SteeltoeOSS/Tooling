FROM <dockerfile.BaseImage>
COPY <dockerfile.BuildPath> <dockerfile.AppPath>
<if(dockerfile.Environment)>
<dockerfile.Environment:{envname |ENV <envname> <dockerfile.Environment.(envname)>}; separator="\n">
<endif>
WORKDIR <dockerfile.AppPath>
CMD dotnet <dockerfile.App>.dll
