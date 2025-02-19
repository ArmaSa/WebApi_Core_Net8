# WebApi .Net 8 

این پروژه نمونه برای ایجاد پروژهای دات نت با فریمورک 8 هست.
## Introduction

🍕 تنظیمات پیشرفته اعمال شده است.

🍟 کنترلر های ابتدایی برای تست و انجام عملیات اضافه شده است.

## Features

1) دیتابیس InMemory
2) الگوی UnitOfWork
3) لایه بندی جداگانه

## Installation

💥 برای نصب پروژه و راه‌اندازی آن، مراحل زیر را دنبال کنید:

1. مخزن را کلون کنید:
    ```bash
    git clone https://github.com/ArmaSa/WebApi_Core_Net8.git
    ```

## Usage Example

✅Admin Role User Example:

{
  "email": "admin@example.com",
  "password": "Admin@123"
}

🧒Add Customer Example:	 
 
{
  "firstName": "Ali",
  "lastName": "Arena",
  "email": "Arena@example.com",
  "fullName": "Ali Arena",
  "phoneNumber": "09345678912",
  "address": "Tehran,..."
}

💸Add Payment:

{
  "customerId": 1,
  "paymentDate": "2025-02-16",
  "amount": 100000000,
  "paymentMethod": 1,
  "invoiceId": 0
}

🛒Add Invoice:

{
  "sodorDate": "2025-02-16",
  "sarResidDate": "2025-03-20",
  "totalAmount": 30500000,
  "paidAmount": 0,
  "status": 1,
  "customerId": 1,
  "invoiceItem": [
    {
      "description": "LED 25 inc",
      "quantity": 2,
      "unitPrice": 15250000
    }
  ]
}
