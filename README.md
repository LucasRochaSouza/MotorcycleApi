# Mottu Project

## Prerequisites

Before you begin, ensure you have the following software installed on your system:

- [Docker](https://docs.docker.com/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install/)

> **Note:** This project was tested using Docker Desktop on Windows.

## Getting Started

To test and run this project, follow these steps:

1. **Build the Docker image**  
   Run the following command in the root directory of the project:

   ```bash
   docker build -t motorcycle .
   ```

2. **Start the services**  
   Use Docker Compose to start the services:

   ```bash
   docker-compose up -d
   ```

This will start the application and its dependencies in detached mode. You can then access the application swagger at http://localhost:8680/swagger.
