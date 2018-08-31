using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hololens_server20180722.Models {
    public class OthelloPlay {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private static List<OthelloPiece> demoPieces;
        public static int AREA_SIZE = 8;

        public JsonCarrier UpdateStatus(JsonCarrier data) {
            log.Info("Start UpdateStatus ");
            var res = data;

            try {
                var initPieces = StoreState(data);
                var lastPosition = data.LastPosition;
                Dictionary<string, OthelloPiece> changePieces = null;

                if (data.RoomNumber == null || data.RoomNumber.Length < 1) {
                    // demo mode
                    /*
                     * デモ画面は固定positionに配置する
                     * pieceの色変化に応じたUnity側エフェクトのデバッグ用
                     */
                    log.Debug("roomNumber is empty ");
                    var tempPieces0 = ChangeState(demoPieces, initPieces);
                    lastPosition = GetLastPosition(tempPieces0, initPieces);
                    var tempPieces1 = CheckReverse(lastPosition, tempPieces0);
                    changePieces = SetIsColliderEnabled(tempPieces1);
                    res.IsMyTurn = false.ToString();
                } else {
                    // play mode
                    log.Debug("roomNumber is  " + data.RoomNumber);
                    if (data.RoomNumber.Contains("temp")) {
                        res.RoomNumber = CreateRoomNumber();
                        res.IsMyTurn = true.ToString();
                    }
                    if (bool.Parse(data.IsMyTurn)) {
                        var tempPieces = CheckReverse(lastPosition, initPieces);
                        changePieces = SetIsColliderEnabled(tempPieces);
                        res.IsMyTurn = SetNextTurn(lastPosition, changePieces);

                    } else {
                        var compPlay = new CompPlay();
                        var tempPieces0 = compPlay.ChangeStateComp(lastPosition, initPieces);
                        lastPosition = GetLastPosition(tempPieces0, initPieces);
                        var tempPieces1 = CheckReverse(lastPosition, tempPieces0);
                        changePieces = SetIsColliderEnabled(tempPieces1);
                        res.IsMyTurn = true.ToString();
                    }
                }

                //othelloPieces = SetIsColliderEnabled(othelloPieces);

                res.PieacesState = PiecesStateToList(changePieces);

            } catch (Exception ex) {
                log.Warn("fail UpdateStatus " + ex.StackTrace.ToString());
                res.ErrorMessage = ex.StackTrace.ToString();
            }

            log.Info("End UpdateStatus ");
            return res;
        }

        private string CreateRoomNumber() {
            var result = "";
            var dateTime = DateTime.Now;
            result = dateTime.ToString("yyyyMMddHHmmssfff");
            return result;
        }

        private string SetNextTurn(string lastPosition, Dictionary<string, OthelloPiece> othelloPieces) {
            var result = false;
            var lastColor = othelloPieces[lastPosition].color;
            if (lastColor == 0) {
                result = true;
            }
            return result.ToString();
        }

        private Dictionary<string, OthelloPiece> SetIsColliderEnabled(Dictionary<string, OthelloPiece> othelloPieces) {

            var result = new Dictionary<string, OthelloPiece>();
            
            foreach (var piece in othelloPieces) {
                if (result.ContainsKey(piece.Key)) {
                    continue;
                } else {
                    result.Add(piece.Key, piece.Value);

                    if (piece.Value.color != 0) {
                        foreach (var adjPos in GetAdjcentPositions(piece.Value.position)) {
                            if (othelloPieces[adjPos].color == 0) {
                                var temp = new OthelloPiece(othelloPieces[adjPos].position, othelloPieces[adjPos].color.ToString(), true.ToString());
                                if (result.ContainsKey(adjPos)) {
                                    continue;
                                }
                                result.Add(adjPos, temp);
                            }
                        }
                    }
                }
            }

            log.Debug(result.Count);
            return result;
        }

        private List<string> PiecesStateToList(Dictionary<string, OthelloPiece> pieces) {
            var result = new List<string>();
            foreach(var piece in pieces) {
                var temp = piece.Value.position + "\t" + piece.Value.color + "\t" + piece.Value.isColliderEnabled;
                result.Add(temp);
            }
            return result;
        }

        private Dictionary<string, OthelloPiece> StoreState(JsonCarrier data) {
            var result = new Dictionary<string, OthelloPiece>();
            foreach (var pieceState in data.PieacesState) {
                //log.Debug(pieceState);
                var index1 = pieceState.IndexOf("\t");
                var index2 = pieceState.IndexOf("\t", index1 + 1);

                var position = pieceState.Substring(0, index1);
                var color = pieceState.Substring(index1 + 1, (index2 - index1 -1));
                var isColliderEnabled = pieceState.Substring(index2 + 1);

                var piece = new OthelloPiece(position, color, isColliderEnabled);
                result.Add(position, piece);
            }
            return result;
        }

        private Dictionary<string, OthelloPiece> ChangeState(List<OthelloPiece> demoPieces, Dictionary<string, OthelloPiece> othelloPieces) {
            var result = new Dictionary<string, OthelloPiece>();

            foreach(var piece in othelloPieces) {
                result.Add(piece.Key, new OthelloPiece(piece.Value.position, piece.Value.color.ToString(), piece.Value.isColliderEnabled.ToString()));
            }

            for (var i = 0; i < demoPieces.Count; i++) {
                var demoKey = demoPieces[i].position;
                if (result[demoKey].color != 0) {
                    continue;
                } else {
                    result[demoKey] = demoPieces[i];
                    break;
                }
            }
            return result; // Last setting position
        }

        private string GetLastPosition(Dictionary<string, OthelloPiece> changePieces, Dictionary<string, OthelloPiece> initPieces) {
            var result = "";
            foreach (var key in changePieces.Keys) {
                if (changePieces[key].ValueEquals(initPieces[key])) {
                    continue;
                } else {
                    result = key;
                    break;
                }
            }
            log.Debug("GetLastPosition " + result);
            return result;
        }

        private Dictionary<string, OthelloPiece> CheckReverse(string lastPosition, Dictionary<string, OthelloPiece> othelloPieces) {
            log.Debug("Start CheckReverse " + lastPosition);
            if(lastPosition.Length < 1) {
                log.Debug("No ChangeState Not need CheckReverse ");
                return othelloPieces;
            }
            
            var lastColor = othelloPieces[lastPosition].color;
            var adjcentPositions = GetAdjcentPositions(lastPosition);

            if(lastColor == 0) {
                log.Debug("The last position color is 0 Not need CheckReverse ");
                return othelloPieces;
            }

            foreach (var adjcentPosition in adjcentPositions) {
                var adjColor = othelloPieces[adjcentPosition].color;
                var stackedPos = new List<string>();

                if (lastColor == adjColor || adjColor == 0) {
                    continue;
                } else {
                    stackedPos.Add(lastPosition);
                    CheckNextPosition(lastPosition, adjcentPosition, stackedPos, othelloPieces);
                }
            }

            log.Debug("End CheckReverse ");
            return othelloPieces;
        }

        private bool CheckNextPosition(string previousPos, string currentPos, List<string> stackedPos, Dictionary<string, OthelloPiece> othelloPieces) {
            var lastPos = stackedPos[0];
            var nextPos = GetNextPos(previousPos, currentPos);

            var lastColor = othelloPieces[lastPos].color;
            var nextColor = othelloPieces[nextPos].color;
            //log.Debug("lastColor  nextColor " + lastColor + " " + nextColor);

            if(lastColor == nextColor) {
                var currTemp = othelloPieces[currentPos];
                currTemp.color = currTemp.color == 1 ? 2 : 1;

                for (var i = 1; i < stackedPos.Count; i++) {
                    var pos = stackedPos[i];
                    var temp = othelloPieces[pos];
                    //log.Debug("othelloPieces[pos].color original is " + othelloPieces[pos].color);
                    temp.color = temp.color == 1 ? 2 : 1;
                    othelloPieces[pos] = temp;
                    log.Debug("othelloPieces[pos].color change to " + othelloPieces[pos].color);
                }
                return true;
            } else if (nextColor == 0) {
                log.Debug("reach emply position ");
                return true;
            } else {
                log.Debug("continue search ");
                stackedPos.Add(currentPos);
                CheckNextPosition(currentPos, nextPos, stackedPos, othelloPieces);
            }

            return false;
        }

        private string GetNextPos(string previousPos, string currentPos) {
            var result = "";

            var prevIndex1 = previousPos.IndexOf("-");
            var prevIndex2 = previousPos.IndexOf("-", prevIndex1 + 1);

            var prevLayerIndex = previousPos.Substring(0, prevIndex1).Replace("Layer", "");
            var prevXRowIndex = previousPos.Substring(prevIndex1 + 1, (prevIndex2 - prevIndex1 - 1)).Replace("XRow", "");
            var prevZColIndex = previousPos.Substring(prevIndex2 + 1).Replace("ZCol", "");

            var currIndex1 = currentPos.IndexOf("-");
            var currIndex2 = currentPos.IndexOf("-", currIndex1 + 1);

            var currLayerIndex = currentPos.Substring(0, currIndex1).Replace("Layer", "");
            var currXRowIndex = currentPos.Substring(currIndex1 + 1, (currIndex2 - currIndex1 - 1)).Replace("XRow", "");
            var currZColIndex = currentPos.Substring(currIndex2 + 1).Replace("ZCol", "");

            var nextLayerIndex = GetIndexString(2 * int.Parse(currLayerIndex) - int.Parse(prevLayerIndex));
            var nextXRowIndex = GetIndexString(2 * int.Parse(currXRowIndex) - int.Parse(prevXRowIndex));
            var nextZColIndex = GetIndexString(2 * int.Parse(currZColIndex) - int.Parse(prevZColIndex));

            result = "Layer"+ nextLayerIndex + "-XRow"+ nextXRowIndex + "-ZCol"+ nextZColIndex;

            return result;
        }

        private List<string> GetAdjcentPositions(string position) {
            var result = new SortedSet<string>();

            var index1 = position.IndexOf("-");
            var index2 = position.IndexOf("-", index1 + 1);

            var layerIndex = position.Substring(0, index1).Replace("Layer", "");
            var xRowIndex = position.Substring(index1 + 1, (index2 - index1 - 1)).Replace("XRow", "");
            var zColIndex = position.Substring(index2 + 1).Replace("ZCol", "");
            //log.Debug("layerIndex " + layerIndex + " xRowIndex " + xRowIndex + " zColIndex " + zColIndex);

            foreach(var layer in GetAdjcentIndexs(layerIndex)) {
                foreach(var xRow in GetAdjcentIndexs(xRowIndex)) {
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
            if(index < 1) {
                temp = 1;
            }else if (index > AREA_SIZE) {
                temp = AREA_SIZE;
            }

            if(temp < 10) {
                return ("0" + temp);
            } else {
                return ("" + index);
            }
        }

        static OthelloPlay() {
            demoPieces = new List<OthelloPiece>() {
                new OthelloPiece("Layer05-XRow04-ZCol03","1","false"),
                new OthelloPiece("Layer05-XRow03-ZCol03","2","false"),
                new OthelloPiece("Layer04-XRow04-ZCol06","1","false"),
                new OthelloPiece("Layer03-XRow04-ZCol04","2","false"),
                new OthelloPiece("Layer03-XRow03-ZCol04","1","false"),
                new OthelloPiece("Layer03-XRow04-ZCol06","2","false")
            };
        }
    }
}