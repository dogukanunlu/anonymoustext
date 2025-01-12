Anonim Metin Paylaşım Platformu Projesi

Proje Özellikleri

Bu proje, kullanıcıların anonim olarak metin paylaşabileceği bir platform sunmaktadır. Platformun temel özellikleri şunlardır:

1. Anonim Metin Paylaşımı

Kullanıcılar herhangi bir kayıt veya giriş yapmadan anonim olarak metin gönderebilir.

Metin gönderimi yapıldığında, gönderim zaman bilgisi ile birlikte kaydedilir.

2. Metin Listeleme

Gönderilen metinler, zamanına göre sıralı bir şekilde görüntülenir.

Yeni metinler sayfanın üstünde görünür (en son gönderilen metin en üstte).

Sonsuz kaydırma özelliği ile metinler dinamik olarak yüklenir. Kullanıcılar aşağı kaydırdıkça daha eski metinler yüklenir.

3. Kötü Sözcük Filtresi

Gönderilen metinlerdeki kötü sözcükleri filtrelemek için bir kontrol mekanizması bulunur.

Kötü sözcük listesi bir dosyada saklanır ve uygulama çalışırken liste güncellenirse, uygulamanın yeniden başlatılmasına gerek kalmadan liste otomatik olarak güncellenir.

4. Veri Kayıtı

Gönderilen metinler ve filtrelenecek kötü sözcükler yerel dosya sistemi üzerine kaydedilir.

Her bir gönderim JSON formatında kaydedilir.

Güvenlik Özellikleri

Proje, kullanıcıların güvenliğini sağlamak ve siber súldırılara karşı koruma sağlamak amacıyla çeşitli önlemlerle donatılmıştır:

1. Cross-Site Scripting (XSS) Koruması

Kullanıcı tarafından gönderilen metinler temizlenir (sanitize edilir). Potansiyel olarak zararlı HTML veya JavaScript kodları uygulamaya enjekte edilemez.

Backend tarafında HtmlSanitizer kütüphanesi kullanılarak zararlı içerik temizlenir.

2. CORS (Cross-Origin Resource Sharing) Kontrolü

API sadece belirli kaynaklardan gelen istekleri kabul edecek şekilde konfigüre edilmiştir. Bu sayede, yetkisiz alan adlarından yapılan talepler reddedilir.

3. Girdi Doğrulama

Gönderilen metinlerin uzunluğu ve formatı backend tarafında kontrol edilir. Bu sayede aşırı uzun, boş veya çok büyük boyutlu veriler reddedilir.

4. Dosya Sistemi Güvenliği

Gönderimlerin kaydedildiği ve kötü sözcüklerin bulunduğu dosya dizini uygulama başlatılırken otomatik olarak oluşturulur.

Dosya yollarının güvenli bir şekilde kullanılması sağlanır (Path.Combine kullanılır).

5. Kötü Sözcük Listesi

Kötü sözcükler, boş satırların ve gereksiz karakterlerin yok edilmesiyle optimize edilmiştir.



Kullanım Senaryoları

1. Metin Gönderimi

Kullanıcı bir metin girer ve gönderir.

Sistem, gönderilen metni kötü sözcük listesiyle kontrol eder.

Kötü sözcük bulunursa, kullanıcıya bir hata mesajı dönülür.

Metin temizse, dosya sistemine kaydedilir.

2. Metin Görüntüleme

Kullanıcı sayfayı kaydırdıkça daha eski metinler yüklenir.

Veriler backend API'ından dinamik olarak çekilir ve frontend'de listelenir.



Karşılaşılan Zorluklar

1. Backendi render'da, frontendi vercel'de deployladım. Render'da data klasörüne erişemediği için 500 dönemkte, bu hatayı gideremedim. Ancak kendiniz görmek isterseniz linkler şu şekilde:

frontend: https://anonymoustext-mauve.vercel.app/
backend: https://anonymoustext.onrender.com/

2. Infinite scroll

Başta tüm datayı çekiyordum, sonradan lazy loadingi kullanmam gerektiğini keşfettim.
