using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using UnitTestWriting.Domain;

namespace UnitTestWriting.Tests.Domain;

public class PromoCodeTests
{
    [TestCase(1, false)]
    [TestCase(49, false)]
    [TestCase(50, true)]
    [TestCase(99, true)]
    public void Ctor_ValidDiscount_ReturnsPremiumOnly(int discount,
                                                      bool expectedPremiumOnly)
    {
        // ACT
        var promoCode = new PromoCode(discount, "CODE", TimeSpan.FromDays(10));

        // ASSERT
        promoCode.PremiumOnly.Should().Be(expectedPremiumOnly);
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(100)]
    [TestCase(101)]
    // –еализаци€ бонусного задани€ на написание Ѕонусные 4 балла (также могут быть засчитаны в задании на написание тестов):
    // добавлены негативные тест-кейсы на промокод со скидкой более >=100% и <=0%.
    // ****
    // ƒобавлен параметр на скидку 101%, остальные параметры уже были.
    // ¬ случае передачи скидки в диапазоне >=100% и <=0% конструктор кидает ArgumentOutOfRangeException(nameof(discount))
    // “аким образом добавление дополнительного параметра покрывает запрос на реализацию выше

    public void Ctor_InvalidDiscount_ThrowsArgumentOutOfRangeException(int discount)
    {
        // ACT
        Action act = () => new PromoCode(discount, "CODE", TimeSpan.FromDays(10));

        // ASSERT
        var expectedMessage = new ArgumentOutOfRangeException("discount").Message;
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage(expectedMessage);
    }

    [TestCase(0)]
    [TestCase(59)]
    public void Ctor_InvalidTimeToLive_ThrowsArgumentOutOfRangeException(int timeToLiveSeconds)
    {
        // ACT
        Action act = () => new PromoCode(10, "CODE", TimeSpan.FromSeconds(timeToLiveSeconds));

        // ASSERT
        var expectedMessage = new ArgumentOutOfRangeException("timeToLive").Message;
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage(expectedMessage);
    }

    [Test]
    public void Ctor_ValidParameters_ReturnsNewPromoCode()
    {
        // ARRANGE
        var discount = 10;
        var timeToLive = TimeSpan.FromHours(10);
        var code = "code";

        // ACT
        var actualPromoCode = new PromoCode(discount, code, timeToLive);

        // ASSERT
        actualPromoCode.Should().BeEquivalentTo(
            new
            {
                Discount = discount,
                Code = code.ToUpper(),
                PremiumOnly = false
            });

        actualPromoCode.Id.Should().NotBe(new Guid());

        actualPromoCode.ExpiredAt.Should().BeCloseTo(DateTime.UtcNow.Add(timeToLive), 100.Milliseconds());
    }
}