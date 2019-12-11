# build stage
FROM node:lts-alpine as build-stage
WORKDIR /app
ARG api_base_url=https://poll.har.ink
ENV VUE_APP_API_URL=$api_base_url
COPY ./babypoll/package*.json ./
RUN npm install
COPY ./babypoll .
RUN npm run build

# production stage
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]