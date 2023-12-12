# Rakovku Bot

A joke Telegram bot which encodes every received message with "Rakovku" joke encoding.

Rakovku encoding takes every consonant letter in a string, gets it's index in a list of all consonants and replaces this consonant letter with another consonant by the consonant index but from the end of consonants list.

For now the bot works only with russian letters. Other unknown letters and symbols keep their positions and stay unchanged.

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
* Add support for logging;
* Wrap the bot into docker container (write `Dockerfile` and `docker-compose.yml`).
