version: "3.7"
services:
  <project.Name>:
    build: .
    ports:
      <project.Protocols: {protocol |- "<protocol.Port>:<protocol.Port>"}; separator="\n">
    volumes:
      - .:/<project.Name>
