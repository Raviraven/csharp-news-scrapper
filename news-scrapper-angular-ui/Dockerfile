FROM node:16-alpine3.11 as build

WORKDIR /usr/local/app
COPY . /usr/local/app/

RUN npm install 
RUN npm run build

####################################################

FROM nginx:1.21.4-alpine

COPY --from=build /usr/local/app/dist/news-scrapper-angular-ui /usr/share/nginx/html
COPY /news-scrapper-nginx/default.conf /etc/nginx/conf.d/default.conf

EXPOSE 80