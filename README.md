## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# APIM Developer Web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/APIM%20Developer/das-apim-developer-web?repoName=SkillsFundingAgency%2Fdas-apim-developer-web&branchName=refs%2Fpull%2F58%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2658&repoName=SkillsFundingAgency%2Fdas-apim-developer-web&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-developer-web&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-developer-web)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This project is the web front end for External Users, Providers and Employers to be able to register API keys to use against the externally published APIs. Live site is [developer.apprenticeships.education.gov.uk](https://developer.apprenticeships.education.gov.uk/)

## How It Works

The single site is deployed as three separate instances to provide API management for Provider, Employers 
and External users. Depending on the `AuthType` configuration value set, this will determine what 
authentication mechanisim is used and also the style that is presented on the front end. 
The purpose of this site is to allow the users to create there own API subscription keys and 
refresh subscription keys to access externally available APIs.

## 🚀 Installation

### Pre-Requisites

* .net 8 and any supported IDE for DEV running
* Azure Storage Account - using the emulator or azurite
* Clone of this repository

### Config

It is possible to run the site using the mock server and no authentication. The below appsettings.json assumes you have  

appsettings.json file
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
  "ConfigNames": "SFA.DAS.Apim.Developer.Web",
  "Environment": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": "",
  "AllowedHosts": "*",
  "cdn": {
    "url": "https://das-at-frnt-end.azureedge.net"
  },
  "AuthType": "Provider",//Allowed values are one of the following Provider Employer External
  "StubAuth": "false" //true or false - false will bypass the authentication process
}
```

#### Azure Table Storage config

**Row Key: SFA.DAS.Apim.Developer.Web_1.0**

**Partition Key: LOCAL**

Data:

```json
{
  "ApimDeveloperWeb" :{
    "DocumentationBaseUrl":"developer.apprenticeships.education.gov.uk"
  },
  "EmployerApimDeveloperApi":{
    "BaseUrl":"https://localhost:5031/",
    "key":""
  },
  "ProviderApimDeveloperApi":{
    "BaseUrl":"https://localhost:5031/",
    "key":""
  },
  "ExternalApimDeveloperApi":{
    "BaseUrl":"https://localhost:5031/",
    "key":""
  },
  "ExternalLinks":{
    "ManageApprenticeshipSiteUrl":"https://localhost:5001/",
    "CommitmentsSiteUrl":"https://localhost:5001/",
    "EmployerRecruitmentSiteUrl":"https://localhost:5001/"
  },
  "ProviderSharedUIConfiguration" :{
    "DashboardUrl":"https://localhost:1234"
  }
}
```

### Running

There are three options to running locally:

* Use the mock server
* Use a internal subscription key to APIM
* Run the outer api locally as well - [APIM Developer Outer API](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/ApimDeveloper)

If running the mock server, then change the above config to have the BaseURL in the
`EmployerApimDeveloperApi`, `ProviderApimDeveloperApi` and `ExternalApimDeveloperApi` to use http instead of https. Start the mock server first then run the SFA.DAS.Apim.Developer.Web project.

If using internal subscription key - update `EmployerApimDeveloperApi`, `ProviderApimDeveloperApi` 
and `ExternalApimDeveloperApi` to the environment URL and add the subscription key to the config and run the SFA.DAS.Apim.Developer.Web project 

If running the outer api, keep the config as above and run the SFA.DAS.Apim.Developer.Web project

All configurations assume that you have set `StubAuth:true`. If you wish to run with provider, employer or external authentication then these will need configuring.

## Technologies

* .Net 8
* Mock Server
* REDIS
* NLog
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions

## 🐛 Known Issues

* Do not run in IISExpress
