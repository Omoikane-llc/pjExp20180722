using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hololens_server20180722.Models {
    public class CompPlay {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, OthelloPiece> ChangeStateComp (string lastPosition, Dictionary<string, OthelloPiece> othelloPieces) {
            log.Info("Start ChangeStateComp " + lastPosition);
            var result = new Dictionary<string, OthelloPiece>();

            foreach (var piece in othelloPieces) {
                result.Add(piece.Key, new OthelloPiece(piece.Value.position, piece.Value.color.ToString(), piece.Value.isColliderEnabled.ToString()));
            }

            var adjPositions = GetAdjcentPositions(lastPosition);
            var compPosition = GetCompPosition(adjPositions, result);
            var compPiece = new OthelloPiece(compPosition, "2", false.ToString());
            result.Remove(compPosition);
            result.Add(compPosition, compPiece);

            log.Info("End ChangeStateComp " + compPosition + " " + result[compPosition].color);
            return result;
        }

        private string GetCompPosition(List<string> adjPositions, Dictionary<string, OthelloPiece> othelloPieces) {
            var result = adjPositions[0];
            foreach (var adjPosition in adjPositions) {
                var adjColor = othelloPieces[adjPosition].color;
                if (adjColor.Equals("0")) {
                    result = adjPosition;
                    break;
                }
            }
            return result;
        }

        private List<string> GetAdjcentPositions(string position) {
            var result = new SortedSet<string>();

            var index1 = position.IndexOf("-");
            var index2 = position.IndexOf("-", index1 + 1);

            var layerIndex = position.Substring(0, index1).Replace("Layer", "");
            var xRowIndex = position.Substring(index1 + 1, (index2 - index1 - 1)).Replace("XRow", "");
            var zColIndex = position.Substring(index2 + 1).Replace("ZCol", "");
            log.Debug("layerIndex " + layerIndex + " xRowIndex " + xRowIndex + " zColIndex " + zColIndex);

            foreach (var layer in GetAdjcentIndexs(layerIndex)) {
                foreach (var xRow in GetAdjcentIndexs(xRowIndex)) {
                    foreach (var zCol in GetAdjcentIndexs(zColIndex)) {
                        result.Add("Layer" + layer + "-XRow" + xRow + "-ZCol" + zCol);
                    }
                }
            }
            result.Remove(position);

            log.Debug("GetAdjcentPositions " + result.Count);
            return result.ToList<string>();
        }

        private List<string> GetAdjcentIndexs(string index) {
            var result = new List<string>();

            var indexNum = int.Parse(index);
            result.Add(GetIndexString(indexNum - 1));
            result.Add(GetIndexString(indexNum));
            result.Add(GetIndexString(indexNum + 1));

            return result;
        }

        private string GetIndexString(int index) {
            var temp = index;
            if (index < 1) {
                temp = 1;
            } else if (index > OthelloPlay.AREA_SIZE) {
                temp = OthelloPlay.AREA_SIZE;
            }

            if (temp < 10) {
                return ("0" + temp);
            } else {
                return ("" + index);
            }
        }
    }
}