## FluentPos

FluentPos is a sample project that consumes the microservice framework. You will learn a lot by exploring this project, which is located under the `./fluentpos` folder.


| Services          | Status         |
| ----------------- | -------------- |
| Gateway           | Completed ✔️   |
| Identity          | Completed ✔️   |
| Catalog           | Completed ✔️   |
| Cart              | WIP       🚧   |
| Ordering          | WIP       🚧   |
| Payment           | WIP       🚧   |

## How to Run ?

### Tye
Tye is a super-awesome way to run your applications quickly. The `fluentpos` project already has this support. Simply run the following at the `./fluentpos` directory :

```
make tye
```

That's it! 

This will spin up all the services required. 
- Gateway will be available on `https://localhost:7002`.
- Identity Service will be available on `https://localhost:7001`.
- Catalog Service will be available on `https://localhost:7003`.

To Test these APIs, you can use open up Visual Code from the `./fluentpos` directory, install the `Thunder Client` extension. I have already included the required Test collections at `./fluentpos/thunder-tests`.

> You can find the specification of services under the ./fluentpos/tye.yaml file.
### Docker & Docker-Compose
The `fluentpos` project comes included with the required docker-compose.yaml and makefile file for your reference.

There are some prerequisites for using these included docker-compose.yml files:

1) Make sure you have docker installed (on windows install docker desktop)

2) Create and install an https certificate:

    ```
    dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\cert.pfx -p password!
    ```

    Note that the certificate name and password should match the ones that are mentioned in the docker-compose.yaml file.

3) It's possible that the above step gives you an `A valid HTTPS certificate is already present` error.
   In that case you will have to run the following command, and then  `Re-Run Step #2`.

    ```
     dotnet dev-certs https --clean
    ```

4) Trust the certificate

    ```
     dotnet dev-certs https --trust
    ```
Once your certificate is trusted, simply navigate into the `./fluentpos` folder of the project and run the following command.

```
make docker-up
```

This will spin up all the containers required. Your Gateway URL will be available on `https://localhost:7002`.

To bring down all the `fluentpos` container, simply run the following.

```
make docker-down
```

*Note that the default Docker Images that will be pulled are from my public Image Repository (for eg, `iammukeshm/fluentpos.identity:latest`). You can switch it your variants if required.*