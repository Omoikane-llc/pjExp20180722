using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hololens_server20180722.Models {
    public class OthelloPiece {
        public string position { get; set; }
        public int color { get; set; }
        public bool isColliderEnabled { get; set; }

        public OthelloPiece() {
        }

        public OthelloPiece(string position, string color, string isColliderEnabled) {
            this.position = position;
            this.color = int.Parse(color);
            this.isColliderEnabled = bool.Parse(isColliderEnabled);
        }

        public bool ValueEquals(OthelloPiece piece) {
            if (this.position.Equals(piece.position) && this.color == piece.color && this.isColliderEnabled == piece.isColliderEnabled) {
                return true;
            }
            return false;
        }
    }
}