# .NET Microservices Boilerplate

[![build-test](https://github.com/fullstackhero/dotnet-microservices-boilerplate/actions/workflows/build.yml/badge.svg)](https://github.com/fullstackhero/dotnet-microservices-boilerplate/actions/workflows/build.yml) [![release](https://github.com/fullstackhero/dotnet-microservices-boilerplate/actions/workflows/release.yml/badge.svg)](https://github.com/fullstackhero/dotnet-microservices-boilerplate/actions/workflows/release.yml)

## Roadmap

Currently maintaining on Trello, might move to Github later on.

[View Trello Board](https://trello.com/b/fflwd3rl/dotnet-microservices-boilerplate-roadmap)

## What's done till now?
- Added Building Blocks for Services - Serilog, Swagger, Fluent Validation, MediatR, AutoMapper, and more.
- ThunderClient Support for API Testing - Love this! 🚀 🚀
- YARP API Gateway - Reverse Proxy
- Identity Server 6 - Separate Microservice for serving and validating JWTs based on API Scope and GrantType as Password.
- DapR Integration - Currently using only the state-store component of Dapr for Caching purposes. Should research more on this. 🚀 🚀
- EFCore with PostgreSQL Integration for Identity Server.
- MongoDB Integration for Catalog API Service.
- GitHub Workflow for Building the Projects in Pipeline.
- Exception Handling Middleware
- Semantic Release - Love this! With this, it's easy now to auto-version your applications based on your commits in a semantic way. It even creates GitHub releases for you! 🚀 🚀
- Tye support for running multiple microservices (and Dapr Side-cars) and managing logs.

## How to Run

 Generate Development certificate by running the following:

 ```
dotnet dev-certs https --trust
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\dev_cert.pfx -p password!
dotnet dev-certs https --trust
 ```

### Docker-Compose

 Once the certificates are installed, you will have to build and push the required docker images to your local instance of docker, and run `docker compose up`.

 Note that, each of the service have their own docker-compose file. This helped me in including the services and resources that a particular microservice would need. In case, there are services that has to be shared by multiple microservices, it would go into the docker-compose.yml at the root of the project. 
 
 Now how to enable docker-compose up` to read multiple docker compose files? Simple, Navigate to ./.env file. Here, you see the paths to all the present docker-compose files.

 But first, let's build the docker images. There are 2 ways for this:
  - Navigate to each of the service, api-gateway & identity server and run the following command to build and push the docker image to your local docker-desktop setup.
  ```
  dotnet publish --os linux --arch x64 -c Release -p:PublishProfile=DefaultContainer -p:ContainerImageTags=latest --self-contained
  ```
- Else, to make your lives simpler, I have also included the commands in the Visual Code Tasks. Simply hit CTRL+SHIFT+P and type in Tasks. Here select the required publish:xxxxxx task. This would also push the docker image. 

Once all the images are built and pushed, you can run the `docker-compose up` command at the root of the solution. If everything goes well, all the required containers would spin up and you will have access to https://localhost:5100 AKA, the API Gateway. You can use the Thunder-Tests in Visual Code to test the gateway!
### Tye

To run locally, you can also use the `tye` tool. I use this for rapid development. Simply run the `tye run` command at the root directory. You can view all the application logs at the tye dashboard which is available at http://127.0.0.1:8000
## Contributing
#### PS, Currently not accepting any contributions. Once the project is stable, will start accepting PRs.

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request.



## Community

- Discord [@fullstackhero](https://discord.gg/gdgHRt4mMw)
- Facebook Page [@codewithmukesh](https://facebook.com/codewithmukesh)
- Youtube Channel [@codewithmukesh](https://youtube.com/c/codewithmukesh)

## License

Code released under [the MIT license](https://github.com/fullstackhero/dotnet-microservices-boilerplate/blob/master/LICENSE).

## Support ⭐

Has this Project helped you learn something New? or Helped you at work?
Here are a few ways by which you can support.

-   Leave a star! ⭐
-   Recommend this awesome project to your colleagues. 🥇
-   Do consider endorsing me on LinkedIn for ASP.NET Core - [Connect via LinkedIn](https://codewithmukesh.com/linkedin) 🦸
-   Or, If you want to support this project in the long run, [consider buying me a coffee](https://www.buymeacoffee.com/codewithmukesh)! ☕

<br>

<a href="https://www.buymeacoffee.com/codewithmukesh"><img width="250" alt="black-button" src="https://user-images.githubusercontent.com/31455818/138557309-27587d91-7b82-4cab-96bb-90f4f4e600f1.png" ></a>


