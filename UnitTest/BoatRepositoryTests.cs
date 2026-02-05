using SailClubLibrary.Exceptions;
using SailClubLibrary.Models;
using SailClubLibrary.Services;

namespace UnitTest;

[TestClass]
public sealed class BoatRepositoryTests
{
    [TestMethod]
    public void TestAddBoat_Normal()
    {
        // Arrange
        BoatRepository repo = new BoatRepository();
        int beforeCount = repo.Count;

        // Act 
        repo.AddBoat(new Boat(0, BoatType.TERA, "Super2", "19-h3j", "Motor", 4.3, 6.6, 10.3, "2000"));
        int afterCount = repo.Count;

        // Assert
        Assert.AreNotEqual(beforeCount, afterCount);
        Assert.AreEqual(beforeCount + 1, afterCount);
    }
    [TestMethod]
    public void TestAddBoat_Null()
    {
        // Arrange
        BoatRepository repo = new BoatRepository();

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => repo.AddBoat(null));
    }
    [TestMethod]
    public void TestAddBoat_SailNrAlreadyExists()
    {
        // Arrange
        BoatRepository repo = new BoatRepository();
        Boat originalBoat = new Boat(0, BoatType.TERA, "Super2", "12-h3j", "Motor", 4.3, 6.6, 10.3, "2000");
        repo.AddBoat(originalBoat);

        // Act & Assert
        Boat duplicateBoat = new Boat(1, BoatType.WAYFARER, "TheClone", "12-h3j", "Sail", 3.5, 5.0, 8.0, "2022");

        Assert.ThrowsException<BoatSailnumberExistsException>(() => repo.AddBoat(duplicateBoat));
    }
}