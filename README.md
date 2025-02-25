## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Provider Relationships API

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2Fdas-pr-api?repoName=SkillsFundingAgency%2Fdas-pr-api&branchName=refs%2Fpull%2F82%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3687&repoName=SkillsFundingAgency%2Fdas-pr-api&branchName=refs%2Fpull%2F82%2Fmerge)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-pr-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-pr-api)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)


## About
This API encapsulates provider relationships with employers and their permissions data.

## 🚀 Installation

### Pre-Requisites

* A clone of this repository
* Azurite or similar local storage emulator
* SQL Database
* Visual studio or similar IDE

### Configuration

* Create a Configuration table in your (Development) local storage account.
* Obtain the SFA.DAS.Roatp.Api.json from the das-employer-config and adjust the SqlConnectionString property to match your local setup.
* Add a row to the Configuration table with fields:
  * PartitionKey: LOCAL
  * RowKey: SFA.DAS.PR.Api_1.0
  * Data: {The contents of the `SFA.DAS.PR.Api.json` file}
  * Obtain the [SFA.DAS.PR.Api.json](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-pr-api/SFA.DAS.PR.Api.json) from the das-employer-config for roatp-api repo
* In the SFA.DAS.PR.Api project, add appSettings.Development.json file with following content:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.PR.Api,SFA.DAS.Encoding",
  "EnvironmentName": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": "",
  "AzureWebJobsServiceBus": "<ServiceBus connection string>"
}
```

## Technologies

* .NetCore 8.0
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions
