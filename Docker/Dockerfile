FROM centos:7

MAINTAINER kousokujin

ARG ver=1.0.2.0
ENV TZ Asia/Tokyo
RUN yum update -y && yum install -y wget unzip
RUN yum install -y libicu libunwind
RUN mkdir /var/pso2
RUN wget -P /var/pso2 https://github.com/kousokujin/PSO2emergencyToDiscordCore/releases/download/${ver}/PSO2emergencyToDiscordCore_${ver}_linux-64.zip
RUN unzip -d /var/pso2 /var/pso2/PSO2emergencyToDiscordCore_${ver}_linux-64.zip
RUN chmod 755 /var/pso2/PSO2emergencyToDiscordCore_${ver}_linux-64/binary/*

CMD ["/bin/bash"]
#CMD ["/var/pso2/PSO2emergencyToDiscordCore_1.0.0.0_linux-64/binary/PSO2emergencyToDiscordCore"]
