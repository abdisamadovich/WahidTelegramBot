# 🎓 Telegram Bot Topshirig'i (O'rta daraja) — .NET 8 + Dicebear

## 🎯 Maqsad

Talaba `Telegram bot` yaratadi, u foydalanuvchidan matnli buyruq oladi va Dicebear API orqali mos `avatar (PNG rasm)` yuboradi. Har bir Dicebear uslubi alohida buyruq bo'ladi, va buyruqdan keyingi matn — avatar uchun **seed** (tugun) sifatida ishlatiladi.

---

## ✅ Talablar

### 📌 Bot funksiyasi:

- Foydalanuvchi quyidagi kabi buyruq yuboradi:

  ```
  /fun-emoji Ali
  ```

  Bot Dicebear API orqali quyidagi havolaga so‘rov yuboradi:

  ```
  https://api.dicebear.com/8.x/fun-emoji/png?seed=Ali
  ```

  So‘ngra PNG rasmni foydalanuvchiga yuboradi.

### 📜 Qo‘llab-quvvatlanadigan buyruqlar:

| Buyruq         | Dicebear uslubi  |
|----------------|------------------|
| `/fun-emoji`   | fun-emoji        |
| `/avataaars`   | avataaars        |
| `/bottts`      | bottts           |
| `/pixel-art`   | pixel-art        |

---

## 🔧 Texnik talablar

- Loyiha **.NET 8** va **Telegram.Bot** kutubxonasi yordamida yoziladi.
- Konsol dasturi sifatida ishlaydi.
- `HttpClient` yordamida Dicebear API’dan rasm olinadi.
- Rasm `SendPhotoAsync` yordamida Telegram foydalanuvchisiga yuboriladi.

---

## 💬 Matnli muloqot qoidalari:

- Foydalanuvchi buyruq yuboradi:  
  Misol: `/bottts robot123`  
  → Bot mos Dicebear rasmni yuboradi.

- Agar seed (so‘z) yuborilmasa:
  > "Iltimos, buyruqdan keyin matn (seed) kiriting. Misol: `/fun-emoji Ali`"

- Noto‘g‘ri yoki noma’lum buyruq bo‘lsa:
  > "Noma’lum buyruq. Quyidagilardan birini ishlating: `/fun-emoji`, `/bottts`, `/avataaars`, `/pixel-art`"

- Agar foydalanuvchi oddiy matn yuborsa (buyruqsiz):
  > "Iltimos, avatar olish uchun buyruqdan foydalaning."

---

## ⚠️ Xatoliklarni qayta ishlash

- Dicebear API ishlamay qolsa:
  > "Avatar yaratishda xatolik yuz berdi. Keyinroq urinib ko‘ring."

- Rasm yuborishda muammo chiqsa:
  > "Rasmni yuborishda xatolik yuz berdi."

---

## 🧪 Test misollar

| Kirish           | Natija                                        |
|------------------|-----------------------------------------------|
| `/fun-emoji Alisher` | Alisher so‘zidan yaratilgan PNG rasm yuboriladi     |
| `/bottts`        | "Iltimos, buyruqdan keyin matn (seed) kiriting." |
| `salom`          | "Iltimos, avatar olish uchun buyruqdan foydalaning." |
| `/hello123 test` | "Noma’lum buyruq..."                         |

---

## 🌟 Bonus (ixtiyoriy)

- `/help` buyrug‘ini qo‘shish (foydalanish bo‘yicha ko‘rsatmalar).
- Bir nechta so‘zli seed’larni (masalan: `John Doe`) qo‘llab-quvvatlash.
- Har bir so‘rovni konsolga log qilish: foydalanuvchi ID, buyruq, seed va holat kodi.
