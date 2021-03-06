﻿using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SupaCharge.Core.Patterns;
using SupaCharge.Testing;

namespace SupaCharge.UnitTests.Core.Patterns {
  [TestFixture]
  public class PipelineTest : BaseTestCase {
    [Test]
    public void TestExecuteWithNoStagesDoesNothing() {
      InitPipeline();
      mPipeline.Execute(44);
    }

    [Test]
    public void TestExecuteWithSingleStage() {
      var stage = Mok<IStage<int>>();
      stage.Setup(s => s.Priority).Returns(10);
      stage.Setup(s => s.Execute(44, It.Is<ICancelToken>(t => !t.Cancelled)));
      InitPipeline(stage.Object);
      mPipeline.Execute(44);
    }

    [Test]
    public void TestExecuteWithMultipleStages() {
      var stages = BA(Mok<IStage<int>>(), Mok<IStage<int>>(), Mok<IStage<int>>());
      Array.ForEach(stages, stage => {
                              stage.Setup(s => s.Priority).Returns(10);
                              stage.Setup(s => s.Execute(44, It.Is<ICancelToken>(t => !t.Cancelled)));
                            });
      InitPipeline(stages.Select(s => s.Object).ToArray());
      mPipeline.Execute(44);
    }

    [Test]
    public void TestExecuteWithMultipleStagesWhenOneCancelsToken() {
      var stages = BA(Mok<IStage<int>>(), Mok<IStage<int>>(), Mok<IStage<int>>());
      Array.ForEach(stages, stage => stage.Setup(s => s.Priority).Returns(10));
      stages[0].Setup(s => s.Execute(44, It.Is<ICancelToken>(t => !t.Cancelled)));
      stages[1]
        .Setup(s => s.Execute(44, It.Is<ICancelToken>(t => !t.Cancelled)))
        .Callback<int, ICancelToken>((x, c) => c.Cancel());
      InitPipeline(stages.Select(s => s.Object).ToArray());
      mPipeline.Execute(44);
    }

    [Test]
    public void TestExecuteWithMultipleStagesOrderedCorrectly() {
      var stages = BA(Mok<IStage<int>>(), Mok<IStage<int>>(), Mok<IStage<int>>());
      var idx = 5;
      Array.ForEach(stages, stage => stage.Setup(s => s.Priority).Returns(--idx));
      stages[2].Setup(s => s.Execute(44, It.Is<ICancelToken>(t => !t.Cancelled)));
      stages[1]
        .Setup(s => s.Execute(44, It.Is<ICancelToken>(t => !t.Cancelled)))
        .Callback<int, ICancelToken>((x, c) => c.Cancel());
      InitPipeline(stages.Select(s => s.Object).ToArray());
      mPipeline.Execute(44);
    }

    [Test]
    public void TestExecuteWithMultipleStagesWithExternalTokenWhenOneCancelsToken() {
      var stages = BA(Mok<IStage<int>>(), Mok<IStage<int>>(), Mok<IStage<int>>());
      Array.ForEach(stages, stage => stage.Setup(s => s.Priority).Returns(10));
      stages[0].Setup(s => s.Execute(44, It.Is<ICancelToken>(t => !t.Cancelled)));
      stages[1]
        .Setup(s => s.Execute(44, It.Is<ICancelToken>(t => !t.Cancelled)))
        .Callback<int, ICancelToken>((x, c) => c.Cancel());
      InitPipeline(stages.Select(s => s.Object).ToArray());
      mPipeline.Execute(44, new CancelToken());
    }

    [SetUp]
    public void DoSetup() {
      mPipeline = null;
    }

    private Pipeline<int> mPipeline;

    private void InitPipeline(params IStage<int>[] stages) {
      mPipeline = new Pipeline<int>(stages);
    }
  }
}