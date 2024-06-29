FROM nginx:stable-alpine3.17-slim
EXPOSE 80

RUN rm /etc/nginx/conf.d/default.conf
COPY FinancialChat.Infra/NGINX/*.conf /etc/nginx/conf.d/
