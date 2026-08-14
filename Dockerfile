# ============================================================
# 1. ASAMA — DERLEME
# SDK imaji: derleyici, NuGet, MSBuild... ~800 MB.
# Bu asamanin ciktisindan sadece yayin klasoru alinacak, kendisi atilacak.
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS derleme

WORKDIR /kaynak

# ONCE SADECE PROJE DOSYASI kopyalanir.
# Sebep: Docker her satiri onbellege alinan bir katman olarak isler ve bir katman
# degisince ondan SONRAKI tum katmanlar yeniden calisir.
# Bagimliliklar nadiren, kaynak kod surekli degisir. Bu sirayla, kod degistiginde
# asagidaki yavas restore adimi onbellekten gelir.
COPY IlkApi.csproj .
RUN dotnet restore

# Kaynak kod BUNDAN SONRA gelir; her kod degisikliginde yalnizca buradan
# asagisi yeniden calisir.
COPY . .
RUN dotnet publish -c Release -o /uygulama --no-restore

# ============================================================
# 2. ASAMA — CALISTIRMA
# aspnet imaji: sadece calisma zamani, derleyici yok. ~110 MB.
# Kucuk olmasi sadece disk meselesi degil: imajda olmayan derleyici,
# saldirganin kullanamayacagi derleyicidir.
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS calistirma

WORKDIR /uygulama

# Container varsayilan olarak ROOT calisir. .NET imajlari hazir bir
# ayricaliksiz kullanici tanimlar; uygulamayi onunla calistiriyoruz.
USER $APP_UID

# Sadece yayin ciktisi aliniyor. SDK, NuGet onbellegi, kaynak kod GELMIYOR.
COPY --from=derleme /uygulama .

# .NET 8'den beri container imajlari 80 degil 8080 dinler:
# ayricaliksiz kullanici 1024 altindaki portlara baglanamaz.
EXPOSE 8080

ENTRYPOINT ["dotnet", "IlkApi.dll"]
