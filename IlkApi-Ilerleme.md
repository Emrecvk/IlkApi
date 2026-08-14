# IlkApi — Proje Durumu ve Öğrenme Günlüğü

**Son güncelleme:** 14 Ağustos 2026 (Faz 5.5 sağlamlaştırma + Faz 6 tamamlandı)
**Geliştirici:** Emre Çevik
**Repo:** `git@github.com:Emrecvk/IlkApi.git`

---

## Asistan Rolü (gelecek oturumlar için)

Bu dosyayı bağlam olarak alan asistan, aşağıdaki uzmanlık alanlarına sahip bir **backend + DevOps mentoru** olarak çalışır.

### Backend
- C# / .NET 10, ASP.NET Core, Minimal API, Controller tabanlı yapı
- Entity Framework Core, LINQ, migration yönetimi, sorgu optimizasyonu
- Katmanlı mimari, Dependency Injection, DTO tasarımı, servis soyutlaması
- REST tasarımı, HTTP durum kodları, API sözleşmesi tasarımı
- FluentValidation, merkezi hata yönetimi, yapılandırılmış loglama
- Kimlik doğrulama ve yetkilendirme: JWT, BCrypt, OAuth2/OIDC, rol tabanlı erişim
- Test: birim testi, entegrasyon testi, Testcontainers

### Veritabanı
- PostgreSQL: şema tasarımı, indeksleme, kısıtlar, sorgu planı okuma
- İlişkisel modelleme, normalizasyon, transaction ve izolasyon seviyeleri
- Migration stratejileri, geriye dönük uyumlu şema değişikliği

### DevOps
- Linux / Ubuntu sistem yönetimi, shell, systemd, izinler, ağ temelleri
- Docker, Dockerfile, multi-stage build, imaj optimizasyonu, Docker Compose
- CI/CD: GitHub Actions, otomatik test, build ve deploy hatları
- Nginx / Caddy, reverse proxy, TLS sonlandırma
- IaC: Terraform, Ansible
- Gözlemlenebilirlik: yapılandırılmış log, Prometheus, Grafana, OpenTelemetry
- Kubernetes, konteyner orkestrasyonu
- Bulut: Azure ve AWS temel servisleri

### Güvenlik
- Gizli bilgi yönetimi (user-secrets, ortam değişkenleri, secret manager)
- Şifre hashleme, token güvenliği, kullanıcı numaralandırma önleme
- Bağımlılık güvenlik taraması, tedarik zinciri riski

### Çalışma tarzı
- Adım adım ilerle, her adımın **neden** yapıldığını açıkla
- Zorluk seviyesi hakkında dürüst ol, "kolay" deyip geçme
- Belirli aralıklarla kavramsal özet ver
- Kod profesyonel standartta olsun, yapay stil kısıtı koyma
- Türkçe iletişim, Türkçe değişken/sınıf adları (proje konvansiyonu)
- Kullanıcı kısa yanıt verir ("devam", "tamam") — bu ilerleme onayıdır
- Kavramsal olarak yanlış gelen bir şey olursa kullanıcı hemen itiraz eder, bu beklenen davranış

---

## Proje Özeti

**IlkApi** — oyun kütüphanesi / backlog takip uygulamasının backend REST API'si.

Amaç: backend fundamentals'ı sıfırdan, gerçek bir proje üzerinde öğrenmek. Her yeni konu bu projeye eklenerek öğreniliyor; her hafta yeni proje açılmıyor.

### Kariyer bağlamı
- **Hedef:** Full stack / DevOps (%70-75 ağırlık)
- **Hobi:** 3D oyun geliştirme, Unity/C# (%25-30 ağırlık)
- **Zaman:** Haftada 10-20 saat
- **Strateji:** "Junior DevOps" pozisyonu neredeyse yok. Önce **altyapıdan anlayan backend developer** olarak sektöre gir, 2-3 yıl sonra platform/DevOps tarafına kay.
- **Stack kararı:** C# / .NET (Unity'den gelen C# birikimi + Türkiye'de kurumsal iş hacmi)

---

## Ortam

| Bileşen | Sürüm / Detay |
|---|---|
| İşletim sistemi | Windows + WSL2 |
| Dağıtım | Ubuntu 26.04 LTS (Resolute Raccoon) |
| .NET SDK | 10.0.x (LTS) — Ubuntu kendi deposundan |
| Veritabanı | PostgreSQL 17-alpine (Docker container) |
| Editör | VS Code + WSL uzantısı + C# eklentisi |
| Proje yolu | `~/projeler/IlkApi` |
| Geliştirme portu | `5144` |

### Önemli ortam kararları

- **Dual boot yerine WSL2.** Reboot maliyeti, parça parça çalışma temposunu öldürür. Asıl Linux öğrenimi VPS üzerinde olacak. İleride derinlik gerekirse VM (snapshot avantajı) tercih edilecek, dual boot değil.
- **Docker Desktop değil, Ubuntu'ya doğrudan Docker.** Ekstra katman yok, sunucu ortamına daha benzer, lisans sorusu yok.
- **Projeler `/mnt/c` altında değil, Linux tarafında.** WSL dosya sistemi köprüsü yavaş.
- **C# Dev Kit kaldırıldı**, sade C# eklentisi bırakıldı. Dev Kit solution tabanlı çalışıp WSL senaryolarında Windows `dotnet.exe`'yi çağırıyor.

---

## Kurulu Paketler ve Araçlar

### Sistem (apt)
`git`, `curl`, `wget`, `build-essential`, `unzip`, `dotnet-sdk-10.0`, Docker (`docker-ce`, `docker-compose-plugin`)

### .NET global tool
`dotnet-ef` (PATH'e `~/.dotnet/tools` eklendi, `~/.bashrc` üzerinden)

### NuGet paketleri
| Paket | Sürüm | Amaç |
|---|---|---|
| `Npgsql.EntityFrameworkCore.PostgreSQL` | — | EF Core PostgreSQL sağlayıcısı |
| `Microsoft.EntityFrameworkCore.Design` | — | Migration araçları |
| `FluentValidation.DependencyInjectionExtensions` | 12.0.0 | Doğrulama |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.0 | JWT doğrulama |
| `BCrypt.Net-Next` | 4.0.3 | Şifre hashleme |

### Kaldırılan paketler
`Microsoft.OpenApi` ve `Microsoft.AspNetCore.OpenApi` — source generator uyumsuzluğu (CS0200) ve güvenlik açığı (NU1903). İleride Scalar ile birlikte, sürümler eşleştirilerek geri eklenecek.

---

## Proje Yapısı

```
IlkApi/
├── IlkApi.csproj
├── Program.cs                  # DI kayıtları + middleware + endpoint bağlama
├── Dockerfile                  # Multi-stage: SDK ile derle, runtime ile çalıştır
├── .dockerignore               # bin/, obj/, .env imaja GİRMEZ
├── compose.yaml                # veritabani + api servisleri
├── .env                        # Gizli değerler (gitignore'da)
├── .env.example                # Anahtar şablonu (Git'e girer)
├── .gitignore
├── Modeller/
│   ├── Oyun.cs                 # Veritabanı entity'si
│   └── Kullanici.cs            # Eposta + SifreHash
├── Veri/
│   └── UygulamaDbContext.cs    # DbSet'ler + OnModelCreating
├── Dto/
│   ├── OyunDto.cs              # OyunOku, OyunYaz
│   ├── AuthDto.cs              # KayitIstek, GirisIstek, TokenYanit
│   └── Dogrulayicilar/
│       ├── OyunYazDogrulayici.cs
│       ├── KayitIstekDogrulayici.cs
│       └── GirisIstekDogrulayici.cs
├── Servisler/
│   ├── IOyunServisi.cs / OyunServisi.cs
│   ├── IAuthServisi.cs / AuthServisi.cs
│   └── ITokenServisi.cs / TokenServisi.cs
├── Endpointler/
│   ├── OyunEndpointleri.cs
│   └── AuthEndpointleri.cs
├── Ortak/
│   ├── HataYaniti.cs
│   ├── JwtAyarlari.cs          # Options deseni + acilista dogrulama
│   ├── DogrulamaFiltresi.cs    # Generic IEndpointFilter
│   └── GlobalHataYakalayici.cs # IExceptionHandler
└── Migrations/
    ├── ..._IlkOlusturma.cs
    └── ..._KullaniciEklendi.cs
```

### Katman sorumlulukları

| Katman | Sorumluluk | Bilmediği şey |
|---|---|---|
| **Endpoint** | HTTP: rota, durum kodu, gövde | İş kuralları, SQL |
| **Servis** | İş kuralları, doğrulama, dönüşüm | HTTP diye bir şey olduğu |
| **DbContext** | Veri erişimi | İş kuralları |

**Bağımlılık tek yöne akar:** Endpoint → Servis → DbContext. Servis, endpoint'i bilmez.

---

## Tamamlanan Aşamalar

### Faz 0 — Ortam Kurulumu ✅

WSL2 + Ubuntu, sistem güncelleme, temel araçlar, Git kimliği, SSH anahtarı (Ed25519), GitHub bağlantısı, .NET SDK.

**Öğrenilenler:** kernel/dağıtım ayrımı, `apt` yaşam döngüsü, root vs sudo, `~` kullanıcıya göre değişir, asimetrik şifreleme, `>` ile `>>` farkı, `~/.bashrc` ve PATH.

### Faz 1 — İlk API (bellek içi) ✅

`dotnet new webapi` → şablon temizlendi → kendi endpoint'leri yazıldı. GET (liste), GET (tek), POST, DELETE. `Results` ile açık durum kodu kontrolü.

**Öğrenilenler:** Kestrel, Minimal API, `builder` / `Build()` / `Run()` üçlüsü, middleware kavramı, route constraint (`{id:int}`), model binding, serileştirme (PascalCase → camelCase), `record`, top-level statements, DTO'nun ilk gerekçesi (Id'yi sunucu üretir).

**Kritik deneyim:** `dotnet run` sonrası eklenen kayıt, uygulama yeniden başlayınca kayboldu. Kalıcılık ihtiyacı teoriden değil deneyimden öğrenildi.

### Faz 2 — Kalıcılık (PostgreSQL + EF Core) ✅

DbContext, DbSet, connection string (user-secrets), ilk migration, `dotnet ef database update`.

**Öğrenilenler:** ORM kavramı, `DbContext` yaşam döngüsü, `DbSet<T>`, migration'ın `Up()`/`Down()` yapısı, `__EFMigrationsHistory` tablosu, `SaveChangesAsync()` olmadan verinin sessizce kaydolmaması, `async/await` (Flutter `Future` karşılığı), DI'ın ilk gerçek kullanımı, **gizli bilgi asla koda girmez**.

### Faz 3 — Katmanlı Mimari ✅

`Program.cs` üç katmana ayrıldı: DTO, servis (interface + implementasyon), endpoint (extension method + `MapGroup`).

**Öğrenilenler:** Dependency Inversion (somut sınıfa değil sözleşmeye bağlanmak), model ve DTO ayrımı, projeksiyon (`.Select()` ToList'ten önce → SQL'e çevrilir), servis katmanının HTTP bilmemesi (`bool` döner, 404'ü endpoint çevirir), DI yaşam süreleri:

| Kayıt | Ömür | Kullanım |
|---|---|---|
| `AddScoped` | Her HTTP isteği | Veritabanına dokunan her şey |
| `AddSingleton` | Uygulama boyunca | Durumsuz, pahalı nesneler |
| `AddTransient` | Her istendiğinde | Hafif, durumsuz |

**Refactoring tanımı:** dışarıdan görünen davranışı değiştirmeden iç yapıyı iyileştirmek. Aynı curl çıktısını almak başarı göstergesidir.

### Faz 4 — Doğrulama ve Hata Yönetimi ✅

FluentValidation, generic `DogrulamaFiltresi<T>` (`IEndpointFilter`), `GlobalHataYakalayici` (`IExceptionHandler`), tutarlı `HataYaniti` formatı.

**Öğrenilenler:** doğrulama sınırda yapılır, tüm hatalar birlikte döner (kullanıcı formu 4 kez göndermesin), **hatanın tamamı loga / istemciye genel mesaj** (stack trace sızdırma), yapılandırılmış loglama (`{Yol}` ayrı alan olarak), `UseExceptionHandler` middleware zincirinin en başında olmalı, sunucuda her zaman `DateTime.UtcNow`.

**Şifre politikası notu:** Sadece minimum uzunluk kondu, karmaşıklık kuralı konmadı. Modern rehberler (NIST dahil) karmaşıklık zorunluluğundan vazgeçti — insanları `Parola123!` gibi tahmin edilebilir kalıplara itiyor.

### Faz 5 — Kimlik Doğrulama ✅

Kullanıcı modeli, BCrypt hashleme, JWT üretimi ve doğrulaması, `RequireAuthorization()` ile korumalı endpoint'ler.

**Öğrenilenler:**
- **Authentication (401) vs Authorization (403)** ayrımı
- JWT **imzalı, şifreli değil** — jwt.io'da içeriği okunabilir, hassas bilgi konmaz
- Stateless doğrulama → ölçeklenebilirliğin temeli; bedeli: token iptal edilemez (bu yüzden kısa ömür + refresh token)
- **BCrypt neden SHA256 değil:** SHA256 hızlı olmak için tasarlandı, şifre için felaket. BCrypt kasıtlı yavaş, maliyeti ayarlanabilir, her şifreye rastgele salt ekler
- **Kullanıcı numaralandırma önleme:** "kullanıcı yok" ile "şifre yanlış" ayrımı yapılmaz. GitHub'ın "Repository not found" mesajıyla aynı prensip
- **Race condition:** kodda "bu eposta var mı" kontrolü yetmez, unique index veritabanı seviyesinde olmalı
- `UseAuthentication` mutlaka `UseAuthorization`'dan **önce** — ters yazılırsa sessizce yanlış çalışır
- `SifreHash` alanı DTO ayrımının gerçek gerekçesini gösterdi

**Bilerek eksik bırakılanlar:** refresh token, brute-force koruması (rate limit), eposta doğrulama, şifre sıfırlama, rol tabanlı yetkilendirme.

### Faz 5.5 — Sağlamlaştırma ✅

Faz 5 sonrası kod tarandı; mutlu yolda doğru ama kenar durumlarda hatalı dört nokta bulundu, **hepsi ölçülerek kanıtlandı ve düzeltildi.**

| # | Belirti (ölçülen) | Kök sebep | Düzeltme |
|---|---|---|---|
| 1 | Production'da **her istek 500** | user-secrets sadece Development'ta yüklenir; `Jwt:Anahtar` null; `!` operatörü uyarıyı sustururken kontrol sağlamıyor | `JwtAyarlari` + `ValidateDataAnnotations().ValidateOnStart()` |
| 2 | `POST /giris {}` → 500 | `/giris`'te doğrulama filtresi yok; `Eposta` null gelip `.Trim()` patlıyor | `GirisIstekDogrulayici` + filtre |
| 3 | Giriş süresi: var olan kullanıcı **175 ms**, olmayan **2.3 ms** (75×) | Kullanıcı yoksa `BCrypt.Verify` hiç çağrılmıyor | Kullanıcı yokken sabit sahte hash'e karşı doğrulama |
| 4 | 8 eşzamanlı aynı kayıt → 1×200, **7×500** | Unique ihlali `DbUpdateException` olarak yakalanmıyor | `catch (DbUpdateException) when (... UniqueViolation)` → 409 |

**Doğrulama sonrası:** 1 → açılışta net mesajla durur, 2 → 400 + alan bazlı mesaj, 3 → süreler iç içe, 4 → 1×200 + 7×409.

**Öğrenilenler:**
- **`!` bir kontrol değil, derleyiciye verilen sözdür.** Tutulup tutulmadığına kimse bakmaz
- **Nullable referans tipleri yalnızca derleme zamanı özelliğidir.** JSON deserializer `string` alana rahatça null yazar — tip, gelen veriyi garanti etmez
- **Fail-fast:** hatayı ilk isteğe değil açılışa çek. `AddJwtBearer(lambda)` tembeldir; "uygulama ayağa kalktı" ≠ "konfigürasyon sağlam"
- **Options deseni:** ayarı kullanıldığı yerde okumak yerine sınıfa bağla, sınırda bir kez doğrula (Faz 4'teki "doğrulama sınırda yapılır" ilkesinin konfigürasyon karşılığı)
- **Yan kanal (side channel):** bilgi cevabın içeriğinden değil, cevabı üretme sürecinden sızabilir. Durum kodu aynı olsa da kronometre konuşur
- **Defense in depth iki yönlü çalışmalı:** unique index veriyi korudu ama API yanlış cevap verdi. İkinci katman devreye girdiğinde de sözleşmeye uyulmalı
- **`when` ile dar yakala:** tüm `DbUpdateException`'ları 409'a çevirmek foreign key / bağlantı hatalarını gizler
- **Giriş doğrulamasında şifre politikası tekrarlanmaz** — politika değişince eski şifreli kullanıcılar kilitlenir

**Kritik bağlantı:** 1 numaralı bulgu Docker'ın ön koşuluydu. Container varsayılan olarak Production'da çalışır ve içinde user-secrets yoktur — düzeltilmeseydi hata "Docker'ı yanlış yaptım" sanılacaktı.

### Faz 6 — Docker ✅

PostgreSQL container'a alınmıştı; **uygulamanın kendisi de container'a alındı.** Multi-stage `Dockerfile`, `.dockerignore`, compose'da `api` servisi, `.env` tek kaynak, Production ortamı.

**Sonuç:** `docker compose up -d` → veritabanı sağlıklı olmadan API başlamıyor, migration'lar açılışta uygulanıyor, uygulama `http://localhost:8080` üzerinde Production'da çalışıyor.

**Öğrenilenler (uygulama container'ı):**
- **Multi-stage build:** SDK imajı (1.26 GB) ile derle, sonuç imajı runtime'dan (349 MB) türet. Sonuç: **361 MB**. Küçüklük sadece disk değil — *imajda olmayan derleyici, saldırganın kullanamayacağı derleyicidir*
- **Katman önbelleği ve COPY sırası:** önce `.csproj` + `restore`, sonra `COPY . .`. Kod değişince `restore` önbellekten gelir. Ölçüldü: 16.2 sn → **4.2 sn**
- **Docker içeriği hash'ler, tarihi değil.** `touch` ile önbellek bozulmaz
- **`.dockerignore` Dockerfile'dan önce yazılır.** Yoksa host'un `bin/` ve `obj/` klasörleri imaja girip container içindeki restore çıktısının üstüne yazar → 6 numaralı hatanın container versiyonu
- **Container varsayılan olarak root çalışır.** .NET imajları hazır `app` kullanıcısı (uid 1654) tanımlar → `USER $APP_UID`
- **8080, 80 değil.** .NET 8'den beri container imajları 8080 dinler: ayrıcalıksız kullanıcı 1024 altındaki portlara bağlanamaz
- **Ortam değişkeninde `:` yerine `__`:** `ConnectionStrings__Varsayilan` → `ConnectionStrings:Varsayilan`. Sebep: birçok kabukta `:` geçersiz
- **Container içinde `localhost` container'ın kendisidir.** Compose aynı ağdaki servisleri **servis adıyla** çözer → `Host=veritabani`
- **`depends_on: condition: service_healthy`** — Faz 6'nın ilk yarısında yazılan healthcheck'in asıl karşılığı. `service_started` yetmez
- **Migration container'da `dotnet ef` ile yapılamaz** (SDK yok) → açılışta `MigrateAsync()`. DbContext Scoped olduğu için elle `CreateScope()` gerekir (açılışta HTTP isteği → kapsam yoktur)
- **Minimal imajın bedeli:** `curl`, `wget`, `nc` yok. Bu yüzden `api` servisine compose healthcheck'i konmadı. Doğru çözüm imaja curl kurmak değil, **orkestratörün HTTP probe'u** (K8s bunu dışarıdan yapar). `/saglik` ucu bunun için var

**Öğrenilenler (Faz 6'nın ilk yarısı — veritabanı container'ı):**
- **Container ≠ VM:** container host kernel'ini paylaşır (namespace + cgroup ile izolasyon), VM'in kendi kernel'i vardır
- **Volume:** container dosya sistemi geçicidir, volume kalıcıdır → *durumsuz uygulama, durumlu depolama*. Faz 2'de öğrenilen ayrımın altyapı karşılığı
- `restart: unless-stopped` → `sudo service postgresql start` derdi bitti
- `ports: "host:container"` eşlemesi
- `healthcheck` — container ayakta olmak ≠ servis hazır olmak
- **İmaj sürümü sabitlenir**, `latest` kullanılmaz
- GPG anahtarı + depo imzası: rastgele depo eklemek, o deponun sahibine root yetkisi vermektir
- `docker` grubuna üyelik pratikte root yetkisidir
- **`compose.yaml` belge değil, çalıştırılabilir tanım** → Infrastructure as Code'un en basit hali

---

## Çözülen Hatalar ve Çıkarılan Dersler

Bu bölüm en değerli kısım — hata ayıklama refleksleri burada oluştu.

| # | Belirti | Kök sebep | Ders |
|---|---|---|---|
| 1 | PowerShell'de `curl -i` çalışmadı, `Uri:` sordu | PowerShell'de `curl`, `Invoke-WebRequest` takma adı | Komut beklenmedik davranıyorsa gerçekte ne olduğunu sor: `type curl` / `Get-Command curl` |
| 2 | Yeni endpoint 404 döndü | Kod düzenlendi ama uygulama yeniden başlatılmadı | `dotnet watch run` kullan (Flutter hot reload karşılığı) |
| 3 | `cd ~/projeler` → "No such file or directory" | root olarak girilmişti, `~` = `/root` | `~` sabit değil, kullanıcıya göre değişir. Prompt'ta `#` root, `$` normal kullanıcı |
| 4 | `Repository not found` | GitHub'da repo hiç açılmamıştı | GitHub, olmayan repo ile erişilemeyen repoyu **kasten** aynı raporlar — bilgi sızıntısı önleme |
| 5 | `CS0234: namespace 'Endpointler' does not exist` | Namespace klasörden değil, dosya içindeki `namespace` satırından gelir | C#'ta yol değil namespace bağlar |
| 6 | `CS0200: IOpenApiMediaType.Example is read only` | Source generator ile paket sürümü uyuşmazlığı | **`obj/` veya `bin/` içindeki hata asla senin kodun değildir.** `rm -rf obj bin` → yeniden dene. Ve **sürüm belirtmeden paket ekleme** |
| 7 | POST'ta 500 | `IValidator<OyunYaz>` DI'da kayıtlı değil | **Derleme hatası vs çalışma zamanı hatası:** DI çalışma anında çözülür. Yeni interface yazdıysan kaydını aynı anda yaz |
| 8 | `CS1061: DbContext does not contain 'Oyunlar'` | DbContext düzenlenirken mevcut `DbSet` üzerine yazıldı | Hata mesajındaki **tipe** git, bahsedilen üyeyi ara |
| 9 | VS Code "No .NET SDKs were found" | VS Code Windows tarafında çalışıyordu, WSL'e bağlı değildi | **Yollara bak:** `C:\...` → Windows süreci, `/home/...` → Linux süreci. Karıştıysa yapılandırma yanlış |
| 10 | `28P01: password authentication failed` | user-secrets'ta `1234`, compose'da `gizli123` | Aynı değer iki yerde tanımlıysa er geç ayrışır → `.env` ile tek kaynak |
| 11 | Production'da her istek 500, Development'ta sorunsuz | user-secrets Development'a özel; `!` ile null uyarısı susturulmuş | **Ortamı değiştir, tekrar dene.** "Bende çalışıyor" bir kanıt değil, bir ortam ifadesidir |
| 12 | Bir endpoint 500, kardeşi 400 | `/giris`'e doğrulama filtresi eklenmemiş | **Kardeş endpoint'leri yan yana oku.** Fark varsa gerekçesi olmalı; yoksa unutulmuştur |
| 13 | Durum kodları aynı, süreler 75× farklı | Kullanıcı yokken pahalı hash işlemi atlanıyor | Güvenlik testinde **yalnızca cevabı değil, cevabın maliyetini de** ölç |
| 14 | Eşzamanlı isteklerde 409 yerine 500 | Unique ihlali istisna olarak yakalanmıyor | Tek istekle test etmek yetmez; **eşzamanlılığı bilerek tetikle** (`&` + `wait`) |
| 15 | `touch` sonrası Docker her katmanı önbellekten aldı | Docker dosya tarihine değil **içeriğe** bakar | Önbellek davranışını test ederken gerçek içerik değiştir |
| 16 | Container loglarında `Cannot load library libgssapi_krb5.so.2` | Npgsql Kerberos kütüphanesini arıyor, minimal imajda yok | **Kırmızı satır ≠ hata.** Npgsql bunu yutup devam ediyor; akışın sonucuna bak (1 numaralı prensip) |
| 17 | `curl`, `wget`, `nc` container içinde yok | Runtime imajı kasten minimal | Aracı imaja eklemeden önce sor: bu iş **imajın içinde mi** yapılmalı? |

### Genel hata ayıklama prensipleri

1. **Kırmızı çıktı her zaman hata değildir.** Log seviyesine (`fail` / `warn` / `info`) ve akışın sonucuna bak. İlk migration'da `__EFMigrationsHistory` sorgusunun başarısız olması normaldir — `Done.` satırı asıl sonuçtur.
2. **Katman katman daralt.** `28P01` teşhisinde kullanılan sıra: sunucu çalışıyor mu → içeriden bağlanabiliyor muyum → dışarıdan bağlanabiliyor muyum → istemci hangi kimlik bilgisini gönderiyor?
3. **Terminal her zaman doğruyu söyler, editör söylemeyebilir.** `dotnet build` temizse editör yanılıyordur.
4. **Hata mesajının şablonunu tanı.** `.NET` DI hatası her zaman şu kalıptadır: *X'i üretmek istedim, Y'ye ihtiyacı vardı, Y kayıtlı değil.*
5. **Araç zincirinde takıldığında "bu beni gerçekten bloke ediyor mu" diye sor.** Etmiyorsa not al, devam et. Yoksa gün araç ayarıyla geçer.

---

## Sık Kullanılan Komutlar

### Günlük akış

İki mod var, ikisi de gecerli:

```bash
# GELISTIRME: veritabani container'da, uygulama host'ta (hizli geri bildirim)
cd ~/projeler/IlkApi
docker compose up -d veritabani
dotnet watch run                     # http://localhost:5144 — Development
```

```bash
# URETIME BENZER: her sey container'da
docker compose up -d --build         # http://localhost:8080 — Production
docker compose logs -f api
```

### .NET
```bash
dotnet build
dotnet run
dotnet watch run
dotnet add package <Paket> --version <Surum>
dotnet list package --vulnerable --include-transitive
dotnet clean && rm -rf obj bin
```

### EF Core
```bash
dotnet ef migrations add <Ad>
dotnet ef migrations script        # uretilecek SQL'i gor
dotnet ef database update
dotnet ef migrations remove        # son migration'i geri al (uygulanmamissa)
```

### Docker
```bash
docker compose up -d
docker compose ps
docker compose logs -f veritabani
docker compose down                # container'lari durdur (VERI KALIR)
docker compose down -v             # volume'leri de sil (VERI GIDER — dikkat)
docker exec -it ilkapi-db psql -U emre -d oyunkutuphanesi
docker volume ls

# Imaj ve build
docker compose build api           # imaji yeniden derle
docker images | grep ilkapi        # boyutu gor
docker compose exec api id         # hangi kullanici calisiyor (root olmamali)
docker history ilkapi-api:latest   # katmanlari ve boyutlarini gor

# Imajin ICINE bak (uygulamayi calistirmadan)
docker run --rm --entrypoint sh ilkapi-api:latest -c 'ls /uygulama'
docker run --rm --entrypoint sh ilkapi-api:latest -c 'find / -name ".env" 2>/dev/null'
```

### Gizli bilgi
```bash
dotnet user-secrets list
dotnet user-secrets set "Anahtar" "Deger"
openssl rand -base64 48            # JWT anahtari uret
```

### Test kalıpları
```bash
# Durum kodunu gormek icin -i
curl -i http://localhost:5144/api/oyunlar/1

# Token alip degiskene atma
TOKEN=$(curl -s -X POST http://localhost:5144/api/auth/giris \
  -H "Content-Type: application/json" \
  -d '{"eposta":"emre@ornek.com","sifre":"uzunbirsifre123"}' \
  | python3 -c "import sys,json; print(json.load(sys.stdin)['token'])")

# Korumali endpoint'e istek
curl -i -X POST http://localhost:5144/api/oyunlar \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"ad":"Celeste","cikisYili":2018,"bitirdim":false}'

# Sadece SUREYI olc (yan kanal testi)
curl -s -o /dev/null -w "%{time_total} sn\n" -X POST http://localhost:5144/api/auth/giris \
  -H "Content-Type: application/json" -d '{"eposta":"yok@ornek.com","sifre":"yanlis123"}'

# ESZAMANLI istek (yaris durumu testi): & ile arka plana at, wait ile bekle
for i in $(seq 1 8); do
  curl -s -o /dev/null -w "%{http_code}\n" -X POST http://localhost:5144/api/auth/kayit \
    -H "Content-Type: application/json" \
    -d '{"eposta":"yaris@ornek.com","sifre":"uzunbirsifre123"}' >> /tmp/yaris.txt &
done
wait; sort /tmp/yaris.txt | uniq -c

# Production davranisini yerelde gor (user-secrets YUKLENMEZ)
ASPNETCORE_ENVIRONMENT=Production dotnet run
```

---

## Kavram Sözlüğü

**Linux / sistem:** kernel, dağıtım, shell/bash, root vs sudo, apt, paket deposu, PATH, `~`, mutlak/göreli yol, systemd, GPG imzası

**Git / SSH:** repository, commit, branch, remote, asimetrik şifreleme, Ed25519, `known_hosts`, `.gitignore`

**.NET:** SDK vs runtime, LTS, `.csproj`, NuGet, ASP.NET Core, Kestrel, Minimal API, middleware, Dependency Injection, top-level statements, `record`, serileştirme, source generator, extension method, generic, Options deseni (`IOptions<T>`), fail-fast / `ValidateOnStart`, null-forgiving operatörü (`!`), exception filter (`catch ... when`), ortam (`ASPNETCORE_ENVIRONMENT`)

**Web / HTTP:** GET/POST/PUT/DELETE, durum kodları (200/201/204/400/401/403/404/409/500), endpoint, route, route constraint, model binding, JSON, REST, OpenAPI, CORS, localhost/port

**Veritabanı:** ORM, DbContext, DbSet, migration, connection string, birincil anahtar, unique index, projeksiyon, race condition, transaction

**Güvenlik:** hash vs şifreleme, salt, BCrypt, JWT, claim, bearer token, kullanıcı numaralandırma, yan kanal (timing attack), defense in depth, gizli bilgi yönetimi, bağımlılık açığı (CVE)

**Docker:** imaj, container, katman, volume, bind mount, namespace, cgroup, Compose, healthcheck, port eşleme, registry, multi-stage build, katman önbelleği, build context, `.dockerignore`, `depends_on` koşulu, container ağı ve servis adı çözümlemesi, ayrıcalıksız kullanıcı (`USER`), `ENTRYPOINT`

**Mimari:** katmanlı mimari, DTO, Dependency Inversion, refactoring, stateless/stateful, Infrastructure as Code

---

## Sıradaki Adımlar

### Hemen sırada
1. ~~Uygulamayı container'a al~~ ✅
2. ~~Container ağı — servis adıyla bağlantı~~ ✅
3. ~~`.env` ile tek kaynak~~ ✅
4. ~~Ortam ayrımı — `ASPNETCORE_ENVIRONMENT`~~ ✅
5. **Bir uygulamayı elle VPS'e at** — DevOps merdiveninin atlanan 3. basamağı. Artık container hazır olduğu için deploy da anlamlı (Hetzner ~€4/ay veya Oracle free tier)
6. **Testler** — birim testi + Testcontainers ile entegrasyon testi. Faz 5.5'te elle `curl` ile bulunan dört hata, artık **otomatik testle** sabitlenmeli

### Faz 6 sonrası bilerek bırakılan borçlar
- `api` servisinde compose healthcheck yok (imajda HTTP istemcisi yok — orkestratör işi)
- İmaj etiketi `10.0` (patch sürümüne veya digest'e sabitlenmedi)
- DataProtection anahtarları container ile birlikte yok oluyor — tek kopyada sorun değil, ölçeklenince volume/Redis gerekir
- Migration açılışta uygulanıyor — geriye dönük uyumlu şema değişikliği disiplini gerekiyor

### Yakın vadede
- Testler: birim testi + Testcontainers ile entegrasyon testi
- Kullanıcıya ait oyun listesi (ilişkisel modelleme, foreign key, `Include`)
- Sayfalama, filtreleme, sıralama
- Rate limiting, CORS
- Scalar ile API dokümantasyonu (OpenAPI'yi sürüm uyumlu şekilde geri ekle)
- Refresh token

### DevOps merdiveni — kalan basamaklar
1. ~~Linux + shell~~ ✅
2. ~~Git~~ ✅
3. **Bir uygulamayı elle VPS'e at** ← atlandı, geri dönülmeli (Hetzner ~€4/ay veya Oracle free tier)
4. Nginx/Caddy + TLS + systemd service
5. ~~Docker~~ ✅ (veritabanı + uygulama, multi-stage, compose)
6. CI/CD: GitHub Actions — push'ta test + build + deploy
7. Observability: yapılandırılmış log, Prometheus + Grafana
8. IaC: Terraform + Ansible
9. Kubernetes — **EN SON.** 1-8 olmadan K8s öğrenmek zaman israfı
10. Bulut derinliği: Azure (.NET tarafı) + sertifika (AZ-104)

**Prensip:** Her aracı, o aracın çözdüğü acıyı bizzat çektikten sonra öğren. Sırayı bozmak kavramları ezbere dönüştürür.

---

## 12 Aylık Takvim

| Dönem | Odak |
|---|---|
| **Ay 1-3** | .NET Web API, EF Core, PostgreSQL, auth. Paralelde Linux + Git. Çıktı: gerçek bir CRUD+auth uygulaması |
| **Ay 4-6** | React/TS ile frontend. VPS'e elle deploy → Nginx → Docker → GitHub Actions |
| **Ay 7-9** | Terraform/Ansible, monitoring, bulut sağlayıcı, Go temelleri |
| **Ay 10-12** | Kubernetes, uçtan uca portfolyo projesi, sertifika |

**Not:** Ay 1'in ilk haftasındayız ve Faz 0-5 zaten bitti. Tempo planın önünde.

---

## Öğrenme Yöntemi

1. **Tutorial cehennemine girme.** 40 saatlik kursu baştan sona izlemek öğrenme değil, öğrenme hissi. Oran **%20 izleme, %80 yazma** olsun.
2. **Kavramı öğrenmek için değil, projeyi ilerletmek için kaynağa git.** "EF Core öğreneceğim" diye oturma; "bu listeyi veritabanından çekmem lazım" de, tıkan, sonra sadece o kısmın dokümanını oku.
3. **Tek proje, 3 ay.** IlkApi büyüsün. Her hafta yeni proje açmak en yaygın ilerleme yanılsaması.

### Kaynaklar
- **Linux:** MIT *The Missing Semester of Your CS Education*, OverTheWire Bandit (0-25 arası hedef)
- **.NET:** Microsoft Learn (ana kaynak), Nick Chapsas (YouTube)
- **Harita:** roadmap.sh — **müfredat değil, harita olarak kullan**

### Bedava kaldıraç
Stajda deployment sürecine dokunmayı iste. "Pipeline'a bakabilir miyim, ortam kurulumunda yardım edebilir miyim." Gerçek üretim altyapısına dokunmak, evde kurulan 10 lab'a bedeldir.

---

## Oyun Tarafı (haftada ~4 saat)

**Öncelik: staj projesindeki 2D metroidvania'yı bitir.** 3D'ye atlamak, yarım kalmış iki proje + tamamlanmış sıfır proje demek. Mevcut 2D projede hareket, animasyon ve kamera sistemleri çalışıyor — bitirmek "3D denedim"den kat kat değerli.

### 3D'ye geçince asıl boşluklar (2D'den taşınmayanlar)
- 3D matematik: vektörler, quaternion, dünya/yerel uzay dönüşümleri
- Aydınlatma ve materyal: bake, probe, PBR
- Asset pipeline: Blender'da basit modelleme, UV, import ayarları — çoğu programcının duvara tosladığı yer
- Performans: draw call, LOD, culling

**Yaklaşım:** Büyük 3D proje başlatma. 1-2 haftalık "dikey dilim"ler yap — bir FPS kontrolcüsü, bir gün-gece döngüsü, bir envanter. Her biri bitmiş ve öğretici.

### İki yolun kesişimi
Oyuna leaderboard + bulut kayıt senkronu ekle. Tek özellik, ama gerçek bir backend + gerçek API tasarımı + gerçek deploy problemi verir. Unity build'ini GitHub Actions'ta almak da iyi bir CI egzersizi — ama lisanslama sorunları yüzünden acı verici, 7-8. aya saklanmalı.

---

## Güvenlik Notları

- `.env` **asla** Git'e girmez. Yanına `.env.example` konur (aynı anahtarlar, sahte değerler)
- ~~`compose.yaml` içindeki geliştirme şifresi Git'e gitmiş durumda~~ ✅ değişkenlere taşındı; healthcheck de `$$POSTGRES_USER` / `$$POSTGRES_DB` okuyor (compose'da `$$`, kabuğa tek `$` geçirir)
- **Production'da konfigürasyon eksikse uygulama açılmamalı.** Yarım yapılandırmayla ayağa kalkıp istek başına 500 dönmek, hatayı görünmez kılar (Faz 5.5 / bulgu 1)
- Production'da user-secrets kullanılmaz; ortam değişkeni veya secret manager devreye girer
- JWT anahtarını ele geçiren, istediği kullanıcı adına token üretebilir
- `docker compose down -v` production'da kariyerin en pahalı yazım hatası olabilir
