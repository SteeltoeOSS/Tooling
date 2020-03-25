version: "3.7"
services:
  <project.Name>:
    build: .
    ports:
      <project.Services: {svc|- "<svc.Port>:<svc.Port>"}; separator="\n">
    volumes:
      - .:/<project.Name>
