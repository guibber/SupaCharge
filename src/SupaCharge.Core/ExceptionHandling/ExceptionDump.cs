﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupaCharge.Core.ExceptionHandling {
  public class ExceptionDump {
    public ExceptionDump(Exception error) {
      mError = error;
      ToString();
    }

    private bool HasInnerException(Exception exc) {
      return exc.InnerException != null;
    }

    private string SetUpExceptions(Exception exc) {
      var parts = new List<string>();

      while (exc != null) {
        parts.Add(exc.GetType().ToString());
        parts.Add(exc.Message);
        parts.Add(exc.StackTrace);

        if (HasInnerException(exc)) {
          parts.Add("----- Inner Exception");
        }
        exc = exc.InnerException;
      }
      var result = parts
        .Where(s => s != null)
        .ToArray();

      return string.Join(Environment.NewLine, parts.ToArray());
    }

    public override string ToString() {
      var parts = SetUpExceptions(mError);
        return parts;
    }

    private string GetStack() {
      return mError.StackTrace;
    }

    private Exception mError;
  }
}
