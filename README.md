# HDI Financial Risk Engine

HDI Financial Risk Engine, finansal risk analizi yapan çok kiracılı (multi-tenant) bir platform olarak geliştirilmiş case çalışmasıdır.

Sistem; iş ortaklarından gelen iş konularını, anlaşma bazlı tanımlanan anahtar kelimeler üzerinden analiz eder, risk skoru ve finansal risk tutarı hesaplar ve sonuçları hem API hem de yönetim arayüzü üzerinden sunar.

---

## 🚀 Özellikler

- Multi-tenant (çok kiracılı) yapı
- Anlaşma yönetimi
- İş ortağı yönetimi
- İş konusu giriş sistemi
- Anahtar kelime tabanlı risk analiz motoru
- Finansal risk tutarı hesaplama
- RESTful API
- MVC tabanlı yönetim paneli (WebUI)
- Dashboard (kart + grafik)
- Global exception handling
- Seed data desteği
- Katmanlı mimari

---

## 🏗 Mimari Yapı

Proje katmanlı mimari ile geliştirilmiştir:

- **Domain**
- **Application**
- **Infrastructure**
- **WebApi**
- **WebUI**

### Katmanların Sorumlulukları

- **Domain** → Entity’ler, enum’lar, base class’lar  
- **Application** → DTO, interface, validation, exception  
- **Infrastructure** → EF Core, DbContext, service implementasyonları  
- **WebApi** → REST endpoint’ler, middleware  
- **WebUI** → MVC ekranlar, dashboard  

---

## 🧰 Kullanılan Teknolojiler

- ASP.NET Core
- Entity Framework Core
- SQL Server
- FluentValidation
- Bootstrap
- Chart.js

---

## 📦 Modüller

### 1. Agreements (Anlaşmalar)
Risk oranı ve analiz parametrelerini tutar.

### 2. Business Partners (İş Ortakları)
Anlaşmalara bağlı çalışan partner firmaları tutar.

### 3. Business Topics (İş Konuları)
Partnerlardan gelen iş konularını temsil eder.

### 4. Risk Analysis (Risk Analizi)
İş konusu açıklamasını analiz eder:
- anahtar kelimeleri bulur
- risk skoru hesaplar
- risk seviyesi belirler
- finansal risk tutarını hesaplar

---

## 🧠 Risk Analizi Mantığı

Sistem keyword (anahtar kelime) tabanlı çalışır.

### Akış

1. İş konusu oluşturulur
2. İlgili agreement’a ait keyword’ler alınır
3. Açıklama içinde eşleşmeler bulunur
4. Toplam risk skoru hesaplanır
5. Risk tutarı hesaplanır
6. Kayıt `Analyzed` durumuna geçirilir

### Formül

Risk Tutarı = Toplam Risk Skoru × Base Risk Rate


---

## 🌱 Seed Data

Proje başlangıcında otomatik olarak aşağıdaki veriler eklenir:

- Tenant
- Agreement
- Agreement Keywords
- Business Partner

Bu sayede sistem kurulduktan sonra direkt test edilebilir.

---

## 🔌 API Endpointleri

### Agreements
- GET /api/agreements/{tenantId}
- GET /api/agreements/{tenantId}/{id}
- POST /api/agreements
- PUT /api/agreements
- DELETE /api/agreements/{tenantId}/{id}

### Business Partners
- GET /api/businesspartners/{tenantId}
- GET /api/businesspartners/{tenantId}/{id}
- POST /api/businesspartners
- PUT /api/businesspartners
- DELETE /api/businesspartners/{tenantId}/{id}

### Business Topics
- GET /api/businesstopics/{tenantId}
- GET /api/businesstopics/{tenantId}/{id}
- POST /api/businesstopics
- PUT /api/businesstopics
- DELETE /api/businesstopics/{tenantId}/{id}

### Risk Analysis
- GET /api/riskanalyses/{tenantId}/business-topic/{businessTopicId}
- POST /api/riskanalyses
- DELETE /api/riskanalyses/{tenantId}/{id}

---

## 🖥 UI Ekranları

- Dashboard (kart + grafik)
- Agreements (liste + ekleme)
- Business Partners (liste + ekleme)
- Business Topics (liste + ekleme)
- Risk Analysis tetikleme

---

## ⚙️ Projeyi Çalıştırma

### 1. Connection string ayarla

HDI.FinancialRiskEngine.WebApi/appsettings.json


---

### 2. Migration çalıştır

**powershell
Add-Migration InitialCreate -Project HDI.FinancialRiskEngine.Infrastructure -StartupProject HDI.FinancialRiskEngine.WebApi
Update-Database -Project HDI.FinancialRiskEngine.Infrastructure -StartupProject HDI.FinancialRiskEngine.WebApi

