
# show os version

```bash
lsb_release -a
```

# setup .net envirement (@ ubuntu 20.04)
[source](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)

```bash
# add .net package repository
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
# Install the SDK
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0
```

# init project
[source](https://docs.microsoft.com/en-us/learn/modules/build-web-api-minimal-api/2-what-is-minimal-api)

```bash
dotnet new web -o PizzaStore -f net6.0
cd PizzaStore/

# Install Swagger Packages
dotnet add package Swashbuckle.AspNetCore

```