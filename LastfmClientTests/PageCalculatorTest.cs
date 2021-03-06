﻿using LastfmClient;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests {
  [TestFixture]
  public class PageCalculatorTest {
    LastfmResponse<LastfmUserRecentTrack> response;
    PageCalculator pageCalculator;

    [SetUp]
    public void Setup() {
      response = new LastfmResponse<LastfmUserRecentTrack> { PerPage = 25, TotalPages = 4, TotalRecords = 100 };
      pageCalculator = new PageCalculator();
    }

    [Test]
    public void Calculate_When_Number_Of_Tracks_Is_Within_First_Page() {
      Assert.That(pageCalculator.Calculate(response, 11), Is.EqualTo(1));
    }

    [Test]
    public void Calculate_When_Number_Of_Tracks_Is_Last_Track_On_First_Page() {
      Assert.That(pageCalculator.Calculate(response, 25), Is.EqualTo(1));
    }

    [Test]
    public void Calculate_When_Number_Of_Tracks_Requested_Is_More_Than_Total_Tracks_Take_All_Pages() {
      Assert.That(pageCalculator.Calculate(response, 125), Is.EqualTo(4));
    }

    [Test]
    public void Calculate_When_Number_Of_Tracks_Requested_Falls_Within_A_Page() {
      Assert.That(pageCalculator.Calculate(response, 56), Is.EqualTo(3));
    }
  }
}
