Inventory<IItem> myInventory = new Inventory<IItem>();

IItem swordA = new Sword();
swordA.SetID(1);
IItem swordB = new Sword();
swordB.SetID(1);
IItem hammer = new Hammer();
hammer.SetID(10);

myInventory.AddItem(swordA);
myInventory.AddItem(swordB);
myInventory.AddItem(hammer);

IItem? itemSelected = myInventory.GetItemByID(10);

if(itemSelected != null)
    itemSelected.Use();

myInventory.RemoveItem(10);