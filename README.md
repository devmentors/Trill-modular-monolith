# Trill

## About

Trill is a simple Twitter clone based on [modular framework](https://github.com/devmentors/modular-framework). The overall architecture is mostly built using event-driven approach. The repository also contains Web UI built with Blazor. 

**How to start the solution?**
----------------

Start the infrastructure using [Docker](https://docs.docker.com/get-docker/):

```
docker-compose up -d
```

Start API located under Bootstrapper project:

```
cd src/Bootstrapper/Trill.Bootstrapper
dotnet run
```