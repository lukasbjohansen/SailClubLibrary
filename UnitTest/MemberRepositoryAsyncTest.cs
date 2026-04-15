using SailClubLibrary.Services;
using SailClubLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest;
[TestClass]
[DoNotParallelize]
public class MemberRepositoryAsyncTest
{
    private MemberRepositoryAsync _repo;
    private Member _testMember;

    [TestInitialize]
    public void Setup()
    {
        _repo = new MemberRepositoryAsync();
        _testMember = new Member(
            0,
            "Jens",
            "Peter",
            "99999999",
            "Testvej 1",
            "Testby",
            "JensP@test.dk",
            MemberType.Senior,
            MemberRole.Member,
            null
        );
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _repo.RemoveAsync(_testMember.PhoneNumber);
    }

    [TestMethod]
    public async Task AddTest()
    {
        // Arrange & Act
        await _repo.AddAsync(_testMember);
        Member? found = await _repo.SearchAsync(_testMember.PhoneNumber);

        // Assert
        Assert.IsNotNull(found);
        Assert.AreEqual(_testMember.PhoneNumber, found.PhoneNumber);
    }

    [TestMethod]
    public async Task CountTest()
    {
        int before = await _repo.Count();
        await _repo.AddAsync(_testMember);
        int after = await _repo.Count();

        Assert.AreEqual(before + 1, after);
    }

    [TestMethod]
    public async Task UpdateTest()
    {
        await _repo.AddAsync(_testMember);

        _testMember.FirstName = "Opdateret";
        await _repo.UpdateAsync(_testMember);

        Member? updated = await _repo.SearchAsync(_testMember.PhoneNumber);
        Assert.IsNotNull(updated);
        Assert.AreEqual("Opdateret", updated.FirstName);
    }

    [TestMethod]
    public async Task RemoveTest()
    {
        await _repo.AddAsync(_testMember);
        await _repo.RemoveAsync(_testMember.PhoneNumber);

        Member? found = await _repo.SearchAsync(_testMember.PhoneNumber);
        Assert.IsNull(found);
    }

    [TestMethod]
    public async Task SearchReturnsNullWhenNotFound()
    {
        Member? found = await _repo.SearchAsync("00000000");
        Assert.IsNull(found);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task AddDuplicatePhoneThrowsException()
    {
        await _repo.AddAsync(_testMember);
        await _repo.AddAsync(_testMember);
    }
}
