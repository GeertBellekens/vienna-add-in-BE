namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public static class BDTXsdImporter
    {
        public static void ImportXsd(IImporterContext context)
        {
            var bdtSchema = new BDTSchema(context);
            bdtSchema.CreateBDTs();
        }
    }
}