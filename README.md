# Bot.ChuckNorris

Ce projet à pour but de montrer un cas d'utilisation très simple du [Bot Framework](https://dev.botframework.com/).

Il est possible de connecter ce Bot à [Skype, Facebook, Slack, Microsoft Teams, etc](https://docs.microsoft.com/en-us/bot-framework/portal-configure-channels "Documentation Microsoft")

## Utilisation

- Création d'un bot sur le [portail de Microsoft](https://dev.botframework.com/bots/new)
- Paramétrage du Web.config de l'API :
  - BotId : Nom renseigné
  - MicrosoftAppId : Guid du Bot
  - MicrosoftAppPassword : Mot de passe
  - Dossier de stockage des fichiers json
- Paramétrage du Web.config du scrapper :
  - Dossier de stockage des fichiers json
- Execution du scrapper pour alimenter le json
- Lancement de l'API
- Test de l'API avec le [Bot Framework Emulator](https://docs.microsoft.com/en-us/bot-framework/debug-bots-emulator)
  - Pour déclancher le bot, la phrase doit commencer par "chuck"
  - chuck ping => pong
  - chuck => Random sur toutes les phrases de la BDD
  - chuck {phrase} => Recherche une réponse contenant un mot de la phrase
  - chuck @{nom} => Site le nom dans la réponse
  

- Enjoy

## Source de données

[Site Web Chuck Norris FR](https://www.chucknorrisfacts.fr)

[API Chuck Norris FR](https://www.chucknorrisfacts.fr/api/api)

## Structure du projet

- Bot.ChuckNorris :
  - API exposant le bot

- Bot.ChuckNorris.BusinessServices :
  - "Intelligence" métier... C'est marrant de parler d'inteligence dans ce contexte.

- Bot.ChuckNorris.DataAcces :
  - Sérialisation / Desérialisation des données

- Bot.ChuckNorris.Scraper :
  - Application console permettant de récupérer des phrases pour alimenter la "base de données" locale
    - Le projet est configurer pour télécharger les phrases par paquet de 15 pages pour ne pas saturer le serveur distant

## Techno

 - C# Framework 4.6.2

 - Microsoft bot framework

 - ASP.NET Web API

 - Newtonsoft

 - Autofac

 - NUnit

 - Moq
