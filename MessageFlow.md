# Message Flow in Ecommerce Microservices Architecture

This document describes the key messages exchanged between microservices in an ecommerce architecture using RabbitMQ.

| **Event Name**            | **Source Service**  | **Target Service(s)**       | **Purpose**                                                             |
|---------------------------|---------------------|------------------------------|------------------------------------------------------------------------|
| `ProductCreated`          | Catalog.API         | Inventory.API, Notification  | Notify other services about a new product created.                     |
| `ProductUpdated`          | Catalog.API         | Inventory.API, Notification  | Sync product details (e.g., name, price) with dependent services.      |
| `ProductStockDecreased`   | Inventory.API       | Order.API                    | Confirm that stock has been deducted for a placed order.               |
| `ProductOutOfStock`       | Inventory.API       | Notification, Catalog.API    | Alert system and update catalog about out-of-stock items.              |
| `BasketCheckedOut`        | Basket.API          | Order.API, Payment.API       | Notify services that a customer has checked out their basket.          |
| `OrderCreated`            | Order.API           | Inventory.API, Payment.API   | Deduct stock and trigger payment processing.                           |
| `PaymentProcessed`        | Payment.API         | Order.API                    | Notify Order.API that payment was successfully processed.              |
| `PaymentFailed`           | Payment.API         | Order.API                    | Notify Order.API that payment failed, and mark the order accordingly.  |
| `OrderShipped`            | Shipping.API        | Notification, Order.API      | Notify about the order shipment.                                       |
| `OrderDelivered`          | Shipping.API        | Notification, Order.API      | Update order status and notify the customer upon delivery.             |
| `LowStockDetected`        | Inventory.API       | Notification                 | Alert admins to replenish stock levels.                                |
| `DiscountApplied`         | Discount.API        | Order.API                    | Confirm that a discount was applied to the order.                      |
| `RefundInitiated`         | Payment.API         | Order.API, Notification      | Notify that a refund process has begun for an order.                   |
| `OrderCancelled`          | Order.API           | Payment.API, Inventory.API   | Initiate a refund and restock the canceled order's products.           |

## Notes
- **Message Broker**: Messages are routed through RabbitMQ, decoupling services for scalability and resilience.
- **Durable Messages**: Ensure messages are marked as persistent to prevent data loss during broker restarts.
- **Event Types**: The architecture supports direct, topic-based, or fanout exchanges depending on the event's scope.

---
