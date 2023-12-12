# Rakovku Bot

A joke Telegram bot which encodes every received message with "Rakovku" joke encoding.

Rakovku encoding takes every consonant letter in a string, gets it's index in a list of all consonants and replaces this consonant letter with another consonant by the consonant index but from the end of consonants list.

For now the bot works only with russian letters. Other unknown letters and symbols keep their positions and stay unchanged.

## Features

You can use this bot in `Docker`.
There a `Dockerfile` exists in the repository and you can build your Docker image with it.
Also `docker-compose.yml` file template is created for your convenience to run this bot as a docker compose service.

To run a docker container with the bot do the following:

```bash
# Build the image
docker build -t rakovkubot -f Dockerfile .
# And then run it with a bot token received from botfather
docker run --name rakovkubot --rm -e RAKOVKU_BOT_TOKEN=<replace_with_your_token> -d rakovkubot
```

To run the bot as a docker compose service use the `docker-compose.yml` file inside the repository.
Inside the file paste your bot token on the appropriate line, save the file and run the following command:

```bash
docker compose up -d
```

## Purpose

This little bot has no practical use but can be an illustrative usage example for `Telegram.Bot`, logging and containerizing such applications in `Docker`.
At least this is more interesting than an echo bot.

## A bit of history

"Rakovku" encoding was a little joke, a sort of children fun cypher, created maybe even before the first computer.
The name "Rakovku" itself is not an official name of this fun pseudo cypher, I doubt that this encoding even have an official name.

An year ago I have created similar Telegram bot but in Python, just to learn how to work with `aiogram` library.
Now I want to learn how to create Telegram bots with `DotNet` and `Telegram.bot`.`

## Todo

* Add support for converting not only messages but also text files;
* Add support for latin alphabet;
