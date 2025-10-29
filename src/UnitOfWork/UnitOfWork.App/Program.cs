using UnitOfWork.App.Services;

// ====================================
// Point d'entrée de l'application
// ====================================

// Initialisation des services
var wallet = new WalletService();
var inventory = new InventoryService();
var purchase = new PurchaseService(wallet, inventory);

// Configuration initiale
const string user = "user-1";
const string sku = "sku-ABC";

wallet.SetBalance(user, 100m);
inventory.SetStock(sku, 2);

// ====================================
// Cas 1 : Transaction réussie
// ====================================
Console.WriteLine("=== Cas 1 : Succès ===");
purchase.Purchase(user, sku, 1, 30m);
Console.WriteLine($"Solde final: {wallet.GetBalance(user):0.00}, Stock: {inventory.GetStock(sku)}");
Console.WriteLine();

// ====================================
// Cas 2 : Transaction échouée avec rollback automatique
// ====================================
Console.WriteLine("=== Cas 2 : Échec => Rollback ===");
try
{
    // Tentative d'achat de 5 unités alors qu'il n'en reste qu'1
    purchase.Purchase(user, sku, 5, 10m);
}
catch (Exception ex)
{
    Console.WriteLine($"[Main] Échec attendu: {ex.Message}");
}
Console.WriteLine($"Solde final: {wallet.GetBalance(user):0.00}, Stock: {inventory.GetStock(sku)}");
Console.WriteLine("\n✅ Le rollback a préservé la cohérence des données !");