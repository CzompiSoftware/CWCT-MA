FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

ENV \
    # CWCT-MA version
    CWCTMA_VERSION=4.0.1-build15

VOLUME [ "/data" ]


# Download CWCT-MA binaries
RUN wget https://packages.czompi.cloud/products/czsoft/cwct-ma/$CWCTMA_VERSION.tar.gz -O cwct-ma.tar.gz \
    && tar -ozxf cwct-ma.tar.gz -C /bin \
    && rm cwct-ma.tar.gz
WORKDIR /bin
EXPOSE 80
ENTRYPOINT ["dotnet", "CWCT-MA.dll"]